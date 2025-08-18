import { useWindowViewModel } from '@/features/window/atoms/windowViewModel'
import { ItemId } from '@/lib/api/types/brandedTypes'
import TimeField from '@/lib/components/TimeField'
import { formatDate } from '@/lib/utils/dayjsUtils'
import AddIcon from '@mui/icons-material/Add'
import DeleteIcon from '@mui/icons-material/Delete'
import {
  Box,
  Button,
  Container,
  Divider,
  IconButton,
  List,
  ListItem,
  Paper,
  Stack,
  Typography
} from '@mui/material'
import type { Dayjs } from 'dayjs'
import { useSnackbar } from 'notistack'
import { forwardRef, useEffect, useImperativeHandle } from 'react'
import { useFieldArray } from 'react-hook-form'
import { HistoryPageViewModel } from '../atoms/historyPageViewModel'
import useWorkRecordEditForm from '../hooks/useWorkRecordEditForm'
import type { WorkRecordSaveRequestDto } from '../types/workTimeTypes'

type HistoryItemViewProps = {
  itemId: ItemId | null
  date: Dayjs | null
  onChangeCanSubmit?: (canSubmit: boolean) => void
  onChangeIsDirty?: (isDirty: boolean) => void
} & Pick<HistoryPageViewModel, 'invoke' | 'isInitialized'>

export type HistoryItemViewHandle = { submit: () => void }

const HistoryItemView = forwardRef<HistoryItemViewHandle, HistoryItemViewProps>(
  ({ invoke, isInitialized, itemId, date, onChangeCanSubmit, onChangeIsDirty }, ref) => {
    const { enqueueSnackbar } = useSnackbar()
    const { invoke: invokeWindow } = useWindowViewModel()

    const { form, mutation, workRecordData } = useWorkRecordEditForm({
      viewModel: { invoke, isInitialized },
      itemId: itemId,
      onSuccess: () => {
        enqueueSnackbar('勤務記録を保存しました。', { variant: 'success' })
      },
      onError: () => {
        enqueueSnackbar('勤務記録の保存に失敗しました。', { variant: 'error' })
      }
    })

    // 親から呼べる submit ハンドラを公開
    useImperativeHandle(
      ref,
      () => ({
        submit: async () => {
          const values = form.getValues() as unknown as WorkRecordSaveRequestDto
          await mutation.mutateAsync(values)
        },
        confirmDiscard: async () => {
          if (!form.formState.isDirty) return true
          const result = await invokeWindow('messageBox', {
            title: '確認',
            message: '保存されていない変更があります。移動してもよいですか？',
            buttons: 'OkCancel',
            icon: 'Warning'
          })
          return result === 'Ok'
        }
      }),
      [form, mutation, invokeWindow]
    )

    useEffect(() => {
      onChangeCanSubmit?.(form.formState.isValid && !mutation.isPending)
    }, [form.formState.isValid, mutation.isPending, onChangeCanSubmit])

    useEffect(() => {
      onChangeIsDirty?.(form.formState.isDirty)
    }, [form.formState.isDirty, onChangeIsDirty])

    const timeTrackingInfo = [
      { label: '勤務時間', value: workRecordData?.workTime || 'N/A' },
      { label: '休憩時間', value: workRecordData?.restTime || 'N/A' },
      { label: '残業時間', value: workRecordData?.overtime || 'N/A' }
    ]

    // field array for restRecords
    const { fields, append, remove } = useFieldArray({ control: form.control, name: 'restRecords' })

    if (!date) return null

    return (
      <Container sx={{ display: 'flex', flexDirection: 'column', gap: 3, p: 3 }}>
        <Typography variant="h4" gutterBottom sx={{ px: 2 }}>
          {formatDate(date)}
        </Typography>

        {/* 勤務・休憩・残業情報 */}
        <Paper variant="outlined" sx={{ px: 3, py: 3, display: 'flex', alignItems: 'center' }}>
          <Stack spacing={1} sx={{ width: 1 }}>
            {timeTrackingInfo.map((item) => (
              <Stack direction="row" alignItems="center" key={item.label}>
                <Typography fontWeight="bold" sx={{ width: 200 }}>
                  {item.label}
                </Typography>
                <Typography>{item.value}</Typography>
              </Stack>
            ))}
          </Stack>
        </Paper>

        <Paper variant="outlined" sx={{ p: 2.5, display: 'flex', gap: 2 }}>
          <Stack spacing={3} sx={{ width: 1 }}>
            <Typography variant="h6">出退勤</Typography>
            <Stack direction="row" spacing={2} alignItems="center">
              <TimeField
                control={form.control}
                name="duration.startedOn"
                label="出勤時間"
                baseDate={date}
              />

              <TimeField
                control={form.control}
                name="duration.finishedOn"
                label="退勤時間"
                baseDate={date}
              />
            </Stack>

            <Divider />

            <Box>
              <Stack direction="row" justifyContent="space-between" alignItems="center">
                <Typography variant="h6">休憩</Typography>
                <Button
                  startIcon={<AddIcon />}
                  onClick={() => append({ id: null, duration: { startedOn: '', finishedOn: '' } })}
                >
                  追加
                </Button>
              </Stack>

              <Paper variant="outlined" sx={{ mt: 2, height: 200, overflowY: 'auto', p: 1 }}>
                <List>
                  {fields.map((f, idx) => (
                    <ListItem
                      key={f.id}
                      sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'stretch',
                        gap: 1
                      }}
                    >
                      <Stack direction="row" spacing={2} alignItems="center">
                        <TimeField
                          control={form.control}
                          name={`restRecords.${idx}.duration.startedOn`}
                          label="開始時間"
                          baseDate={date}
                        />

                        <TimeField
                          control={form.control}
                          name={`restRecords.${idx}.duration.finishedOn`}
                          label="終了時間"
                          baseDate={date}
                        />

                        <IconButton edge="end" onClick={() => remove(idx)} aria-label="delete">
                          <DeleteIcon />
                        </IconButton>
                      </Stack>
                    </ListItem>
                  ))}
                </List>
              </Paper>
            </Box>
          </Stack>
        </Paper>
      </Container>
    )
  }
)

export default HistoryItemView
