import { ItemId } from '@/lib/api/types/brandedTypes'
import { useQuery } from '@tanstack/react-query'
import { HistoryPageViewModel } from '../atoms/historyPageViewModel'
import { WorkRecordTallyGetRequestDto } from '../types/workTimeTypes'

type UseGetWorkRecordListQueryOptions = {
  viewModel: Pick<HistoryPageViewModel, 'invoke' | 'isInitialized'>
  options: WorkRecordTallyGetRequestDto
}

export const getWorkRecordListQueryKey = (options?: WorkRecordTallyGetRequestDto) =>
  options ? ['getWorkRecordList', JSON.stringify(options)] : ['getWorkRecordList']

export const useWorkRecordListQuery = ({
  viewModel: { invoke, isInitialized },
  options
}: UseGetWorkRecordListQueryOptions) => {
  return useQuery({
    queryKey: getWorkRecordListQueryKey(options),
    queryFn: () => invoke({ command: 'getWorkRecordList', payload: options }),
    enabled: isInitialized
  })
}

type UseGetWorkRecordQueryOptions = {
  viewModel: Pick<HistoryPageViewModel, 'invoke' | 'isInitialized'>
  itemId: ItemId | null
}

export const getWorkRecordQueryKey = (itemId?: ItemId | null) =>
  itemId ? ['getWorkRecord', itemId] : ['getWorkRecord']

export const useWorkRecordQuery = ({
  viewModel: { invoke, isInitialized },
  itemId
}: UseGetWorkRecordQueryOptions) => {
  return useQuery({
    queryKey: getWorkRecordQueryKey(itemId),
    queryFn: () => (itemId ? invoke({ command: 'getWorkRecord', payload: itemId }) : undefined),
    enabled: isInitialized
  })
}
