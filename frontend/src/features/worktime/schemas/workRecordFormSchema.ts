import dayjs from 'dayjs'
import { z } from 'zod'

export const timeDurationSchema = z
  .object({
    startedOn: z.string().default(''),
    finishedOn: z.string().nullable().default(null)
  })
  .refine(
    (val) => {
      // finishedOn が nullishの場合は許容
      if (!val.finishedOn) return true
      const s = dayjs(val.startedOn).second(0).millisecond(0)
      const f = dayjs(val.finishedOn).second(0).millisecond(0)
      return s.valueOf() <= f.valueOf()
    },
    { message: '開始時刻が終了時刻よりも後に指定されています。', path: ['finishedOn'] }
  )
  .default({ startedOn: '', finishedOn: null })

export const restRecordSchema = z.object({
  id: z.uuid().nullable().default(null),
  duration: timeDurationSchema
})

export const workRecordSaveSchema = z.object({
  id: z.uuid().nullable().default(null),
  duration: timeDurationSchema,
  restRecords: z.array(restRecordSchema).default([])
})

export type WorkRecordSaveRequest = z.infer<typeof workRecordSaveSchema>
