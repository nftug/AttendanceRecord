import { useWindowViewModel } from '@/features/window/atoms/windowViewModel'
import { ItemId } from '@/lib/api/types/brandedTypes'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { HistoryPageViewModel } from '../atoms/historyPageViewModel'
import { getWorkRecordListQueryKey } from './historyPageQueries'

type UseWorkRecordDeleteOptions = {
  viewModel: Pick<HistoryPageViewModel, 'invoke' | 'isInitialized'>
  itemId: ItemId | null
  onSuccess?: () => void
  onError?: (error: unknown) => void
}

const useAskDeleteWorkRecord = ({
  viewModel,
  itemId,
  onSuccess,
  onError
}: UseWorkRecordDeleteOptions) => {
  const queryClient = useQueryClient()
  const { invoke: invokeWindow } = useWindowViewModel()

  const mutation = useMutation({
    mutationFn: async () => {
      if (!itemId) throw new Error('Item ID is required')
      await viewModel.invoke('deleteWorkRecord', itemId)
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: getWorkRecordListQueryKey() })
      onSuccess?.()
    },
    onError
  })

  const askAndDelete = async () => {
    const answer = await invokeWindow('messageBox', {
      title: '削除の確認',
      message: 'この記録を削除してもよろしいですか？',
      buttons: 'YesNo',
      icon: 'Warning'
    })
    if (answer === 'Yes') mutation.mutate()
  }

  return askAndDelete
}

export default useAskDeleteWorkRecord
