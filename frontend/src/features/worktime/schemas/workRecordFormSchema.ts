import { DateTimeString, ItemId } from '@/lib/api/types/brandedTypes'
import dayjs from 'dayjs'
import { z } from 'zod'

const timeDurationSchema = z
  .object({
    startedOn: z.string().nonempty().brand<DateTimeString>(),
    finishedOn: z.string().nullable().brand<DateTimeString>()
  })
  .refine(
    (val) => {
      // finishedOn が null の場合は許容
      if (val.finishedOn === null) return true
      const s = dayjs(val.startedOn).second(0).millisecond(0)
      const f = dayjs(val.finishedOn).second(0).millisecond(0)
      if (!s.isValid() || !f.isValid()) return false
      return s.valueOf() <= f.valueOf()
    },
    { message: '開始時刻が終了時刻よりも後に指定されています。', path: ['finishedOn'] }
  )

const restRecordSchema = z.object({
  id: z.string().nullable().brand<ItemId>(),
  duration: timeDurationSchema
})

export const workRecordSaveSchema = z.object({
  id: z.string().nullable().brand<ItemId>(),
  duration: timeDurationSchema,
  restRecords: z.array(restRecordSchema)
})

export type WorkRecordSaveRequest = z.infer<typeof workRecordSaveSchema>
