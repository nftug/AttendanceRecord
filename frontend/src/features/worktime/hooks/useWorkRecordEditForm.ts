import { ItemId } from '@/lib/api/types/brandedTypes'
import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useEffect } from 'react'
import { useForm } from 'react-hook-form'
import { HistoryPageViewModel } from '../atoms/historyPageViewModel'
import { workRecordSaveSchema } from '../schemas/workRecordFormSchema'
import { WorkRecordSaveRequestDto } from '../types/workTimeTypes'
import {
  getWorkRecordListQueryKey,
  getWorkRecordQueryKey,
  useWorkRecordQuery
} from './historyPageQueries'

type UseWorkRecordEditFormOptions = {
  viewModel: Pick<HistoryPageViewModel, 'invoke' | 'isInitialized'>
  itemId: ItemId | null
  onSuccess?: () => void
  onError?: (error: unknown) => void
}

const useWorkRecordEditForm = ({
  viewModel,
  itemId,
  onSuccess,
  onError
}: UseWorkRecordEditFormOptions) => {
  const queryClient = useQueryClient()
  const form = useForm({ resolver: zodResolver(workRecordSaveSchema), mode: 'onChange' })
  const { data: workRecordData, isLoading } = useWorkRecordQuery({ viewModel, itemId })

  useEffect(() => {
    form.reset(
      workRecordData ?? { id: null, duration: { startedOn: '', finishedOn: null }, restRecords: [] }
    )
  }, [workRecordData, form])

  const mutation = useMutation({
    mutationFn: (formData: WorkRecordSaveRequestDto) =>
      viewModel.invoke({ command: 'saveWorkRecord', payload: formData }),
    onSuccess: async () => {
      await Promise.all([
        queryClient.invalidateQueries({ queryKey: getWorkRecordQueryKey() }),
        queryClient.invalidateQueries({ queryKey: getWorkRecordListQueryKey() })
      ])
      onSuccess?.()
    },
    onError
  })

  return { form, workRecordData, isLoading, mutation }
}

export default useWorkRecordEditForm
