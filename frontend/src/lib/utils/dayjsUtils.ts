import dayjs, { Dayjs } from 'dayjs'
import { DateTimeString } from '../api/types/brandedTypes'

export const formatDateTime = (datetime: DateTimeString | Dayjs, template?: string) =>
  dayjs(datetime).format(template || 'YYYY/MM/DD HH:mm:ss')

export const formatDate = (date: DateTimeString | Dayjs) => dayjs(date).format('YYYY/MM/DD')

export const formatTime = (datetime: DateTimeString | Dayjs) => dayjs(datetime).format('HH:mm:ss')

export const toDateTimeString = (time: Dayjs) =>
  time.format('YYYY-MM-DDTHH:mm:ssZ') as DateTimeString
