import { standardSchemaResolver } from '@hookform/resolvers/standard-schema'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useSnackbar } from 'notistack'
import { useCallback, useEffect, useMemo } from 'react'
import { useForm } from 'react-hook-form'
import { useAppConfigViewModel } from '../atoms/appConfigViewModel'
import { AppConfigFormValues, appConfigSchema } from '../schemas/appConfigSchema'
import { AppConfigSaveRequestDto } from '../types/appConfigTypes'

const defaultValues: AppConfigFormValues = {
  standardWorkMinutes: 0,
  residentNotificationEnabled: false,
  workEndAlarm: {
    isEnabled: false,
    remainingMinutes: 0,
    snoozeMinutes: 0
  },
  restStartAlarm: {
    isEnabled: false,
    elapsedMinutes: 0,
    snoozeMinutes: 0
  },
  statusFormat: {
    statusFormat: '',
    timeSpanFormat: ''
  }
}

const useAppConfigSettings = () => {
  const viewModel = useAppConfigViewModel()
  const { enqueueSnackbar } = useSnackbar()
  const queryClient = useQueryClient()

  const { control, handleSubmit, register, reset, watch, formState } = useForm<AppConfigFormValues>(
    {
      defaultValues,
      mode: 'onChange',
      resolver: standardSchemaResolver(appConfigSchema)
    }
  )

  const queryKey = useMemo(
    () => ['settings', 'appConfig', viewModel.viewId] as const,
    [viewModel.viewId]
  )

  const query = useQuery({
    queryKey,
    enabled: viewModel.isInitialized,
    queryFn: () => viewModel.invoke({ command: 'getAppConfig' })
  })

  const handleReset = useCallback(() => {
    if (query.data) {
      const { workEndAlarm, restStartAlarm, statusFormat, ...rest } = query.data
      reset({
        ...rest,
        workEndAlarm: {
          isEnabled: workEndAlarm.isEnabled,
          remainingMinutes: workEndAlarm.remainingMinutes,
          snoozeMinutes: workEndAlarm.snoozeMinutes
        },
        restStartAlarm: {
          isEnabled: restStartAlarm.isEnabled,
          elapsedMinutes: restStartAlarm.elapsedMinutes,
          snoozeMinutes: restStartAlarm.snoozeMinutes
        },
        statusFormat: {
          statusFormat: statusFormat.statusFormat,
          timeSpanFormat: statusFormat.timeSpanFormat
        }
      })
    }
  }, [query.data, reset])

  useEffect(() => {
    handleReset()
  }, [handleReset])

  const mutation = useMutation({
    mutationFn: (input: AppConfigFormValues) =>
      viewModel.invoke({ command: 'saveAppConfig', payload: input as AppConfigSaveRequestDto }),
    onSuccess: async () => {
      await queryClient.invalidateQueries({ queryKey })
      enqueueSnackbar('アプリ設定を保存しました。', { variant: 'success' })
    },
    onError: () => {
      enqueueSnackbar('アプリ設定の保存に失敗しました。', { variant: 'error' })
    }
  })

  const onSubmit = handleSubmit(async (values) => {
    await mutation.mutateAsync(values)
  })

  const isWorkEndEnabled = watch('workEndAlarm.isEnabled')
  const isRestStartEnabled = watch('restStartAlarm.isEnabled')

  return {
    query,
    mutation,
    control,
    register,
    formState,
    isWorkEndEnabled,
    isRestStartEnabled,
    onSubmit,
    handleReset
  }
}

export default useAppConfigSettings
