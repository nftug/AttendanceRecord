export interface IpcMessenger {
  sendMessage(message: string): void
  receiveMessage(callback: (message: string) => void): void
}

export type ViewId = ReturnType<typeof crypto.randomUUID> & { readonly __brand: 'ViewId' }

export type CommandId = ReturnType<typeof crypto.randomUUID> & { readonly __brand: 'CommandId' }

export type EventMessage = { viewId: ViewId; commandId?: CommandId } & EventEnvelope

export type EventEnvelope = {
  event: string
  payload?: unknown
  commandName?: string
}

export type CommandMessage = { viewId: ViewId; commandId?: CommandId } & CommandEnvelope

export type CommandEnvelope = { command: string; payload?: unknown }
