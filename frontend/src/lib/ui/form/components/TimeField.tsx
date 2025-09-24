import { TimePicker } from '@mui/x-date-pickers'
import dayjs, { Dayjs } from 'dayjs'
import { Control, Controller, FieldValues, Path } from 'react-hook-form'
import { toDateTimeString } from '../../../utils/dayjsUtils'

type TimeFieldProps<T extends FieldValues> = {
  control: Control<T>
  name: Path<T> | string
  label?: string
  baseDate: Dayjs | null
}

const toDateStringWithBase = (time: Dayjs | null, baseDate: Dayjs | null) => {
  if (!time) return null
  if (!baseDate) return toDateTimeString(time)

  const date = baseDate.hour(time.hour()).minute(time.minute()).second(0).millisecond(0)
  return toDateTimeString(date)
}

export default function TimeField<T extends FieldValues>({
  control,
  name,
  label,
  baseDate
}: TimeFieldProps<T>) {
  return (
    <Controller
      control={control}
      name={name as Path<T>}
      render={({ field, fieldState: { error } }) => (
        <TimePicker
          value={field.value ? dayjs(field.value) : null}
          onChange={(val: Dayjs | null) => field.onChange(toDateStringWithBase(val, baseDate))}
          label={label}
          ampm={false}
          slotProps={{
            textField: {
              error: !!error,
              helperText: error?.message ?? undefined
            }
          }}
        />
      )}
    />
  )
}
