import { TimeSpanString } from '@/lib/api/types/timeTypes'

export type CurrentWorkRecordStateDto = {
  currentDateTime: string
  workTime: TimeSpanString
  restTime: TimeSpanString
  overtime: TimeSpanString
  overtimeMonthly: TimeSpanString
  isWorking: boolean
  isActive: boolean
  isResting: boolean
}
