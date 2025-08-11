import { createContext, Dispatch, SetStateAction, useContext } from 'react'

export const DrawerContext = createContext<boolean>(false)

export const DrawerDispatchContext = createContext<Dispatch<SetStateAction<boolean>>>(() => {})

export const useDrawerContext = () => useContext(DrawerContext)

export const useDrawerDispatchContext = () => useContext(DrawerDispatchContext)
