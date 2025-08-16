import { DateString, DateTimeString, ItemId, TimeSpanString } from '@/lib/api/types/brandedTypes'

export type TimeDuration = { startedOn: DateTimeString; finishedOn?: DateTimeString }

export type YearAndMonth = { year: number; month: number }

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
  recordedAt: YearAndMonth
  totalWorkTime: TimeSpanString
  totalRestTime: TimeSpanString
  totalOvertime: TimeSpanString
  workRecords: { id: ItemId; date: DateString }[]
}

export type WorkRecordTallyGetRequestDto = YearAndMonth

export type WorkRecordResponseDto = {
  id: ItemId
  recordedDate: DateString
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
  recordedDate: DateString
  duration: TimeDuration
  isActive: boolean
}

export type WorkRecordSaveRequestDto = {
  id: ItemId | null
  duration: TimeDuration
  restRecords: { id: ItemId | null; duration: TimeDuration }[]
}
