import { standardSchemaResolver } from '@hookform/resolvers/standard-schema'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { useSnackbar } from 'notistack'
import { useCallback, useEffect, useMemo } from 'react'
import { useForm } from 'react-hook-form'
import { useAppConfigViewModel } from '../atoms/appConfigViewModel'
import { AppConfigFormValues, appConfigSchema } from '../schemas/appConfigSchema'
import { AppConfigSaveRequestDto } from '../types/appConfigTypes'

const useAppConfigSettings = () => {
  const viewModel = useAppConfigViewModel()
  const { enqueueSnackbar } = useSnackbar()
  const queryClient = useQueryClient()

  const { control, handleSubmit, register, reset, watch, formState } = useForm<AppConfigFormValues>(
    {
      mode: 'onChange',
      resolver: standardSchemaResolver(appConfigSchema)
    }
  )

  const queryKey = useMemo(() => ['settings', 'appConfig'] as const, [])

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
    } else {
      reset(appConfigSchema.parse({}))
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

  return {
    query,
    mutation,
    control,
    register,
    formState,
    watch,
    onSubmit,
    handleReset
  }
}

export default useAppConfigSettings
