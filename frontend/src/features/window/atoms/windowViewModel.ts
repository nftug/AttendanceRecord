import useViewModel, { useProvideViewModel } from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue } from 'jotai'
import { WindowCommandEnvelope, WindowEventEnvelope } from '../types/windowTypes'

const windowViewModelAtom = atom<WindowViewModel>()

export const useWindowViewModel = () =>
  useViewModel<WindowEventEnvelope, WindowCommandEnvelope>('window')

export type WindowViewModel = ReturnType<typeof useWindowViewModel>

export const useWindowViewModelAtom = () => useAtomValue(windowViewModelAtom)

export const useProvideWindowViewModel = () =>
  useProvideViewModel(useWindowViewModel, windowViewModelAtom)
