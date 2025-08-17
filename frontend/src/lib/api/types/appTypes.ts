import { CommandId } from './apiTypes'

// Default events and commands
export type AppEventEnvelope =
  | { event: 'receive:init'; commandName: 'init'; payload: { type: string } }
  | {
      event: 'error'
      payload: ViewModelErrorEventResult
    }

export type AppCommandEnvelope =
  | { command: 'init'; payload: { type: string } }
  | { command: 'leave' }

// DTO
export type ViewModelErrorEventResult = {
  commandId?: CommandId
  message: string
  details?: string
}
