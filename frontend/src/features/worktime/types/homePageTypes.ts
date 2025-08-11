import { CurrentWorkRecordStateDto } from './workTimeTypes'

export type HomePageCommands = { command: 'toggleWork' } | { command: 'toggleRest' }

export type HomePageEvents =
  | {
      event: 'receive:toggleWork'
      payload: CurrentWorkRecordStateDto
      commandName: 'toggleWork'
    }
  | {
      event: 'receive:toggleRest'
      payload: CurrentWorkRecordStateDto
      commandName: 'toggleRest'
    }
  | {
      event: 'state'
      payload: CurrentWorkRecordStateDto
    }
