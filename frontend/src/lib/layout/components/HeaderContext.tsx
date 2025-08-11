import { useState } from 'react'
import { DrawerContext, DrawerDispatchContext } from '../contexts/DrawerContext'

export const HeaderProvider = ({ children }: { children?: React.ReactNode }) => {
  const [drawerOpened, setDrawerOpened] = useState(false)

  return (
    <DrawerContext.Provider value={drawerOpened}>
      <DrawerDispatchContext.Provider value={setDrawerOpened}>
        {children}
      </DrawerDispatchContext.Provider>
    </DrawerContext.Provider>
  )
}
