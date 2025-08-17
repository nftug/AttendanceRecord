import { DateTimeString, ItemId, TimeSpanString } from '@/lib/api/types/brandedTypes'

export type TimeDuration = { startedOn: DateTimeString; finishedOn?: DateTimeString }

export type CurrentWorkRecordStateDto = {
  currentDateTime: DateTimeString
  workTime: TimeSpanString
  restTime: TimeSpanString
  overtime: TimeSpanString
  overtimeMonthly: TimeSpanString
  isWorking: boolean
  isActive: boolean
  isResting: boolean
}

export type WorkRecordTallyResponseDto = {
  recordedDate: DateTimeString | null
  totalWorkTime: TimeSpanString
  totalRestTime: TimeSpanString
  totalOvertime: TimeSpanString
  workRecords: WorkRecordListItem[]
}

export type WorkRecordListItem = { id: ItemId; date: DateTimeString }

export type WorkRecordItemEditOption = { id: ItemId | null; date: DateTimeString }

export type WorkRecordTallyGetRequestDto = { recordedMonthDate: DateTimeString }

export type WorkRecordResponseDto = {
  id: ItemId
  recordedDate: DateTimeString
  duration: TimeDuration
  restRecords: RestRecordResponseDto[]
  workTime: TimeSpanString
  restTime: TimeSpanString
  overtime: TimeSpanString
  isTodaysOngoing: boolean
  isWorking: boolean
  isResting: boolean
}

export type RestRecordResponseDto = {
  id: ItemId
  recordedDate: DateTimeString
  duration: TimeDuration
  isActive: boolean
}

export type WorkRecordSaveRequestDto = {
  id: ItemId | null
  duration: TimeDuration
  restRecords: { id: ItemId | null; duration: TimeDuration }[]
}
