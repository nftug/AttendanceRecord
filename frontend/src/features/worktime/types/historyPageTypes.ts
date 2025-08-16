import { ItemId } from '@/lib/api/types/brandedTypes'
import {
  WorkRecordResponseDto,
  WorkRecordSaveRequestDto,
  WorkRecordTallyGetRequestDto,
  WorkRecordTallyResponseDto
} from './workTimeTypes'

export type HistoryPageCommand =
  | {
      command: 'getWorkRecordList'
      payload: WorkRecordTallyGetRequestDto
    }
  | {
      command: 'getWorkRecord'
      payload: ItemId
    }
  | {
      command: 'saveWorkRecord'
      payload: WorkRecordSaveRequestDto
    }
  | {
      command: 'deleteWorkRecord'
      payload: ItemId
    }

export type HistoryPageEvent =
  | {
      event: 'receive:getWorkRecordList'
      commandName: 'getWorkRecordList'
      payload: WorkRecordTallyResponseDto
    }
  | {
      event: 'receive:getWorkRecord'
      commandName: 'getWorkRecord'
      payload: WorkRecordResponseDto
    }
  | {
      event: 'receive:saveWorkRecord'
      commandName: 'saveWorkRecord'
    }
  | {
      event: 'receive:deleteWorkRecord'
      commandName: 'deleteWorkRecord'
    }
