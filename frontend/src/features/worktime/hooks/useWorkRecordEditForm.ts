import { ItemId } from '@/lib/api/types/brandedTypes'
import useResourceEditForm from '@/lib/ui/form/hooks/useResourceEditForm'
import { useQueryClient } from '@tanstack/react-query'
import { HistoryPageViewModel } from '../atoms/historyPageViewModel'
import { createDefaultWorkRecord, workRecordSaveSchema } from '../schemas/workRecordFormSchema'
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
  const { data: workRecordData, isLoading } = useWorkRecordQuery({ viewModel, itemId })

  const { form, mutation } = useResourceEditForm({
    resourceData: workRecordData,
    schema: workRecordSaveSchema,
    toFormFields: (data) => data ?? createDefaultWorkRecord(),
    saveFn: (formData) => viewModel.invoke({ command: 'saveWorkRecord', payload: formData }),
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
