import useViewModel, { useProvideViewModel } from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue } from 'jotai'
import { HomePageCommands, HomePageEvents } from '../types/homePageTypes'

const homePageViewModelAtom = atom<HomePageViewModel>()

export const useHomePageViewModel = () => {
  const viewModel = useViewModel<HomePageEvents, HomePageCommands>('homePage')
  const state = viewModel.useViewState('state')
  return state ? { state, ...viewModel } : undefined
}

export type HomePageViewModel = NonNullable<ReturnType<typeof useHomePageViewModel>>

export const useHomePageViewModelAtom = () => useAtomValue(homePageViewModelAtom)

export const useProvideHomePageViewModel = () =>
  useProvideViewModel(useHomePageViewModel, homePageViewModelAtom)
