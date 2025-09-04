import dayjs from 'dayjs'
import { z } from 'zod'

export const timeDurationSchema = z
  .object({
    startedOn: z.string().nonempty(),
    finishedOn: z.string().nullable()
  })
  .refine(
    (val) => {
      // finishedOn が nullishの場合は許容
      if (!val.finishedOn) return true
      const s = dayjs(val.startedOn).second(0).millisecond(0)
      const f = dayjs(val.finishedOn).second(0).millisecond(0)
      if (!s.isValid() || !f.isValid()) return false
      return s.valueOf() <= f.valueOf()
    },
    { message: '開始時刻が終了時刻よりも後に指定されています。', path: ['finishedOn'] }
  )

export const createDefaultTimeDuration = () => ({ startedOn: '', finishedOn: null })

export const restRecordSchema = z.object({
  id: z.string().nullable(),
  duration: timeDurationSchema
})

export const createDefaultRestRecord = () => ({ id: null, duration: createDefaultTimeDuration() })

export const workRecordSaveSchema = z.object({
  id: z.string().nullable(),
  duration: timeDurationSchema,
  restRecords: z.array(restRecordSchema)
})

export const createDefaultWorkRecord = () => ({
  id: null,
  duration: createDefaultTimeDuration(),
  restRecords: []
})

export type WorkRecordSaveRequest = z.infer<typeof workRecordSaveSchema>
