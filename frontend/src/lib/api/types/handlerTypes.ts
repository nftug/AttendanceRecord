import { CommandEnvelope, CommandId, EventEnvelope, ViewId } from './apiTypes'

export type EmitterKey = `state:${ViewId}:${string}` | `result:${CommandId}:${string}`

export type EmitterKeyOptions =
  | { viewId: ViewId; event: string }
  | { commandId: CommandId; commandName: string }

export type Event<T extends EventEnvelope, TName extends T['event']> = Extract<T, { event: TName }>

export type Command<T extends CommandEnvelope, TName extends T['command']> = Extract<
  T,
  { command: TName }
>

export type CommandResult<T extends EventEnvelope, TName extends T['commandName']> = Extract<
  T,
  { commandName: TName }
>

export type CommandArguments<TCommand extends CommandEnvelope, TName extends TCommand['command']> =
  Extract<TCommand, { command: TName }> extends { payload: infer P } ? [payload: P] : []
