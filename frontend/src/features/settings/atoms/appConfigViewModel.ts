import useViewModel, { useProvideViewModel } from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue } from 'jotai'
import { AppConfigCommands, AppConfigEvents } from '../types/appConfigTypes'

const appConfigViewModelAtom = atom<AppConfigViewModel>()

export const useAppConfigViewModel = () =>
  useViewModel<AppConfigEvents, AppConfigCommands>('appConfig')

export type AppConfigViewModel = ReturnType<typeof useAppConfigViewModel>

export const useAppConfigViewModelAtom = () => useAtomValue(appConfigViewModelAtom)

export const useProvideAppConfigViewModel = () =>
  useProvideViewModel(useAppConfigViewModel, appConfigViewModelAtom)
