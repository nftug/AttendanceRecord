import { useWindowViewModel } from '@/features/window/atoms/windowViewModel'
import { useSnackbar } from 'notistack'
import { useEffect } from 'react'
import { useAlarmViewModel } from '../atoms/alarmViewModel'

const AlarmView = () => {
  const { subscribe, dispatch } = useAlarmViewModel()
  const { invoke: invokeWindow } = useWindowViewModel()
  const { enqueueSnackbar } = useSnackbar()

  useEffect(() =>
    subscribe('triggered', async ({ type }) => {
      await invokeWindow('setMinimized', false)

      const messageTitle = type === 'WorkEnd' ? '勤務終了のアラーム' : '休憩開始のアラーム'
      const messageContent = type === 'WorkEnd' ? '勤務時間終了前です。' : '休憩時間になりました。'

      const result = await invokeWindow('messageBox', {
        title: messageTitle,
        message: `${messageContent}\nスヌーズするには「キャンセル」を選択してください。`,
        buttons: 'OkCancel'
      })

      if (result === 'Cancel') {
        dispatch('snooze', { type })
        enqueueSnackbar('スヌーズしました。', { variant: 'info' })
      }
    })
  )

  return null
}

export default AlarmView
