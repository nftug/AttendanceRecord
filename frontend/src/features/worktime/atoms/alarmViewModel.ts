import useViewModel, { useProvideViewModel } from '@/lib/api/hooks/useViewModel'
import { atom, useAtomValue } from 'jotai'
import { AlarmCommands, AlarmEvents } from '../types/alarmViewTypes'

const alarmViewModelAtom = atom<AlarmViewModel>()

export const useAlarmViewModel = () => useViewModel<AlarmEvents, AlarmCommands>('alarm')

export type AlarmViewModel = ReturnType<typeof useAlarmViewModel>

export const useAlarmViewModelAtom = () => useAtomValue(alarmViewModelAtom)

export const useProvideAlarmViewModel = () =>
  useProvideViewModel(useAlarmViewModel, alarmViewModelAtom)
