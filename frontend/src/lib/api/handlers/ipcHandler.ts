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
import { CommandResult, EmitterKey, EmitterKeyOptions, Event } from '../types/handlerTypes'
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
  return <TName extends TCommand['command']>(input: Extract<TCommand, { command: TName }>) => {
    const message: CommandMessage = { viewId, ...input }
    ipcMessenger.sendMessage(JSON.stringify(message))
  }
}

export const createInvoker = <TEvent extends EventEnvelope, TCommand extends CommandEnvelope>(
  viewId: ViewId
) => {
  return <TName extends TCommand['command'] & CommandResult<TEvent, string>['commandName']>(
    input: Extract<TCommand, { command: TName }>
  ): Promise<CommandResult<TEvent, TName>['payload']> => {
    const commandId = crypto.randomUUID() as CommandId
    const message: CommandMessage = { viewId, commandId, ...input }

    return new Promise<CommandResult<TEvent, TName>['payload']>((resolve, reject) => {
      const key = generateEmitterKey({ commandName: input.command, commandId })

      const removeSubscription = eventEmitter.on(key, (payload) => {
        removeAllSubscriptions()
        resolve(payload)
      })

      const appSubscribe = createSubscriber<AppEventEnvelope>(viewId)
      const removeErrorSubscription = appSubscribe('error', (payload) => {
        if (payload.commandId === commandId) {
          removeAllSubscriptions()
          reject(new ViewModelError(viewId, payload))
        }
      })

      const removeAllSubscriptions = () => {
        removeSubscription()
        removeErrorSubscription()
      }

      ipcMessenger.sendMessage(JSON.stringify(message))
    })
  }
}

export const createInitViewHandler = (viewId: ViewId, viewType: string) => {
  const dispatch = createDispatcher<AppCommandEnvelope>(viewId)
  const invoke = createInvoker<AppEventEnvelope, AppCommandEnvelope>(viewId)
  const subscribe = createSubscriber<AppEventEnvelope>(viewId)

  const disposeErrorLogger = subscribe('error', (payload) => {
    // Ignore errors with a commandId
    if (payload.commandId) return

    const error = new ViewModelError(viewId, payload)
    console.error(error)
    console.error(error.details)
  })

  const handlePageHide = () => dispatch({ command: 'leave' })

  return {
    init: async () => {
      await invoke({ command: 'init', payload: { type: viewType } })
      window.addEventListener('pagehide', handlePageHide)
    },
    dispose: () => {
      dispatch({ command: 'leave' })
      disposeErrorLogger()
      window.removeEventListener('pagehide', handlePageHide)
    }
  }
}
