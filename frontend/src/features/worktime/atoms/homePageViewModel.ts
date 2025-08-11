import useViewModel, { useProvideViewModel } from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue } from 'jotai'
import { HomePageCommands, HomePageEvents } from '../types/homePageTypes'

const useHomePageViewModelInternal = () => {
  const viewModel = useViewModel<HomePageEvents, HomePageCommands>('homePage')
  const state = viewModel.useViewState('state')
  return state ? { state, ...viewModel } : undefined
}

export type HomePageViewModel = NonNullable<ReturnType<typeof useHomePageViewModelInternal>>

const homePageViewModelAtom = atom<HomePageViewModel>()

export const useHomePageViewModel = () => useAtomValue(homePageViewModelAtom)

export const useProvideHomePageViewModel = () =>
  useProvideViewModel(useHomePageViewModelInternal, homePageViewModelAtom)
