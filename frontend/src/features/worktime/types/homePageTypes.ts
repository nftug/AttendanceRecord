import { CurrentWorkRecordStateDto } from './workTimeTypes'

export type HomePageCommands = { command: 'toggleWork' } | { command: 'toggleRest' }

export type HomePageEvents =
  | {
      event: 'receive:toggleWork'
      commandName: 'toggleWork'
    }
  | {
      event: 'receive:toggleRest'
      commandName: 'toggleRest'
    }
  | {
      event: 'state'
      payload: CurrentWorkRecordStateDto
    }
