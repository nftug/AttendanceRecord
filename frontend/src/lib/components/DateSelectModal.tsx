import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material'
import { DateCalendar } from '@mui/x-date-pickers/DateCalendar'
import type { Dayjs } from 'dayjs'
import { useEffect, useState } from 'react'
import { createCallable } from 'react-call'

type DateSelectModalProps = {
  initialDate?: Dayjs | null
}

const DateSelectModal = createCallable<DateSelectModalProps, Dayjs | null>(
  ({ call, initialDate }) => {
    const [open, onClose] = [!call.ended, () => call.end(null)]
    const [value, setValue] = useState<Dayjs | null>(initialDate ?? null)

    useEffect(() => {
      setValue(initialDate ?? null)
    }, [initialDate, open])

    return (
      <Dialog open={open} onClose={onClose} fullWidth maxWidth="xs">
        <DialogTitle>日付を選択</DialogTitle>
        <DialogContent>
          <Box sx={{ mt: 1 }}>
            <DateCalendar value={value} onChange={(d) => setValue(d)} />
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>キャンセル</Button>
          <Button onClick={() => call.end(value)} disabled={!value}>
            選択
          </Button>
        </DialogActions>
      </Dialog>
    )
  },
  500
)

export default DateSelectModal
