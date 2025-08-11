import { camelCase } from 'change-case'
import { createNanoEvents } from 'nanoevents'
import { match, P } from 'ts-pattern'
import {
  CommandEnvelope,
  CommandId,
  CommandMessage,
  EventEnvelope,
  EventMessage,
  IpcMessenger,
  ViewId
} from '../types/apiTypes'
import { AppCommandEnvelope, AppEventEnvelope } from '../types/appTypes'
import {
  Command,
  CommandArguments,
  CommandResult,
  EmitterKey,
  EmitterKeyOptions,
  Event
} from '../types/handlerTypes'
import { ViewModelError } from '../types/viewModelError'

let ipcMessenger: IpcMessenger

const eventEmitter = createNanoEvents<Record<EmitterKey, (payload: unknown) => void>>()

const generateEmitterKey = (opts: EmitterKeyOptions | EventMessage) =>
  match(opts)
    .returnType<EmitterKey>()
    .with(
      { commandName: P.nonNullable, commandId: P.nonNullable },
      ({ commandName, commandId }) => `result:${commandId}:${camelCase(commandName)}`
    )
    .otherwise(({ viewId, event }) => `state:${viewId}:${camelCase(event)}`)

export const initializeIpcHandler = (messenger: IpcMessenger) => {
  ipcMessenger = messenger

  ipcMessenger.receiveMessage((json) => {
    const message = JSON.parse(json) as EventMessage
    eventEmitter.emit(generateEmitterKey(message), message.payload)
  })
}

export const createSubscriber = <TEvent extends EventEnvelope>(viewId: ViewId) => {
  return <TName extends TEvent['event']>(
    eventName: TName,
    callback: (payload: Event<TEvent, TName>['payload']) => void
  ) => {
    const key = generateEmitterKey({ viewId, event: eventName })
    return eventEmitter.on(key, (payload) => callback(payload as Event<TEvent, TName>['payload']))
  }
}

export const createDispatcher = <TCommand extends CommandEnvelope>(viewId: ViewId) => {
  return <TName extends TCommand['command']>(
    commandName: TName,
    ...args: CommandArguments<TCommand, TName>
  ) => {
    const message: CommandMessage = {
      viewId,
      command: commandName,
      payload: args[0]
    }
    ipcMessenger.sendMessage(JSON.stringify(message))
  }
}

export const createInvoker = <TEvent extends EventEnvelope, TCommand extends CommandEnvelope>(
  viewId: ViewId
) => {
  return <TName extends TCommand['command'] & CommandResult<TEvent, string>['commandName']>(
    commandName: TName,
    ...args: CommandArguments<TCommand, TName>
  ): Promise<Command<TCommand, TName>['payload']> => {
    const commandId = crypto.randomUUID() as CommandId
    const message: CommandMessage = {
      viewId,
      command: commandName,
      payload: args[0],
      commandId
    }

    return new Promise<CommandResult<TEvent, TName>['payload']>((resolve) => {
      const key = generateEmitterKey({ commandName, commandId })
      const unsubscribe = eventEmitter.on(key, (payload) => {
        unsubscribe()
        resolve(payload)
      })
      ipcMessenger.sendMessage(JSON.stringify(message))
    })
  }
}

export const initView = (viewId: ViewId, viewType: string) => {
  const dispatch = createDispatcher<AppCommandEnvelope>(viewId)
  const subscribe = createSubscriber<AppEventEnvelope>(viewId)

  dispatch('init', { type: viewType })

  const disposeErrorLogger = subscribe('error', (payload) => {
    const error = new ViewModelError(viewId, payload)
    console.error(error)
    console.error(error.details)
  })

  const handlePageHide = () => dispatch('leave')
  window.addEventListener('pagehide', handlePageHide)

  return () => {
    dispatch('leave')
    disposeErrorLogger()
    window.removeEventListener('pagehide', handlePageHide)
  }
}
