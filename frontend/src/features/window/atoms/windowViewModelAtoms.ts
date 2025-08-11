import useViewModel, { useProvideViewModel } from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue } from 'jotai'
import { WindowCommandEnvelope, WindowEventEnvelope } from '../types/windowTypes'

const windowViewModelAtom = atom<WindowViewModel>()

const useWindowViewModelInternal = () =>
  useViewModel<WindowEventEnvelope, WindowCommandEnvelope>('window')

export type WindowViewModel = ReturnType<typeof useWindowViewModelInternal>

export const useWindowViewModel = () => useAtomValue(windowViewModelAtom)

export const useProvideWindowViewModel = () =>
  useProvideViewModel(useWindowViewModelInternal, windowViewModelAtom)
