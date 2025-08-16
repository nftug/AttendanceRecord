import useViewModel, { useProvideViewModel } from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue } from 'jotai'
import { HistoryPageCommand, HistoryPageEvent } from '../types/historyPageTypes'

const historyPageViewModelAtom = atom<HistoryPageViewModel>()

export const useHistoryPageViewModel = () =>
  useViewModel<HistoryPageEvent, HistoryPageCommand>('historyPage')

export type HistoryPageViewModel = ReturnType<typeof useHistoryPageViewModel>

export const useHistoryPageViewModelAtom = () => useAtomValue(historyPageViewModelAtom)

export const useProvideHistoryPageViewModel = () =>
  useProvideViewModel(useHistoryPageViewModel, historyPageViewModelAtom)
