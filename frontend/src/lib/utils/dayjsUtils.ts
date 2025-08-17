import dayjs, { Dayjs } from 'dayjs'
import { DateString, DateTimeString } from '../api/types/brandedTypes'

export const formatDateTime = (datetime: DateTimeString | Dayjs, template?: string) =>
  dayjs(datetime).format(template || 'YYYY/MM/DD HH:mm:ss')

export const formatDate = (date: DateString | Dayjs) => dayjs(date).format('YYYY/MM/DD')

export const formatTime = (datetime: DateTimeString | Dayjs) => dayjs(datetime).format('HH:mm:ss')

export const toIsoWithDate = (time: Dayjs | null, baseDate: Dayjs | null) => {
  if (!time) return null
  if (!baseDate) return time.toISOString()
  return baseDate
    .hour(time.hour())
    .minute(time.minute())
    .second(time.second())
    .millisecond(0)
    .toISOString()
}
