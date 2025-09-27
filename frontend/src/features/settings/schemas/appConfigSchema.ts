import { z } from 'zod'

const integerNumber = (minValue: number, message: string) =>
  z.coerce
    .number('数値を入力してください。')
    .int('整数を入力してください。')
    .default(minValue)
    .refine((value) => value >= minValue, message)

const nonNegativeInt = integerNumber(0, '0以上の値を入力してください。')
const positiveInt = integerNumber(1, '1以上の値を入力してください。')

export const appConfigSchema = z.object({
  standardWorkMinutes: nonNegativeInt,
  workEndAlarm: z
    .object({
      isEnabled: z.boolean(),
      remainingMinutes: nonNegativeInt,
      snoozeMinutes: positiveInt
    })
    .default({ isEnabled: false, remainingMinutes: 0, snoozeMinutes: 1 }),
  restStartAlarm: z
    .object({
      isEnabled: z.boolean(),
      elapsedMinutes: nonNegativeInt,
      snoozeMinutes: positiveInt
    })
    .default({ isEnabled: false, elapsedMinutes: 0, snoozeMinutes: 1 }),
  statusFormat: z
    .object({
      statusFormat: z.string().max(100, '100文字以内で入力してください。'),
      timeSpanFormat: z.string().max(100, '100文字以内で入力してください。')
    })
    .default({ statusFormat: '', timeSpanFormat: '' })
})

export type AppConfigFormValues = z.infer<typeof appConfigSchema>
