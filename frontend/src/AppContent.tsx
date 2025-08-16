import { Box } from '@mui/material'
import { Route, Routes } from 'react-router-dom'
import { useWindowViewModelAtom } from './features/window/atoms/windowViewModel'
import AlarmView from './features/worktime/components/AlarmView'
import { HeaderProvider } from './lib/layout/components/HeaderContext'
import TheDrawer from './lib/layout/components/TheDrawer'
import TheHeader from './lib/layout/components/TheHeader'
import AboutPage from './pages/AboutPage'
import IndexPage from './pages/IndexPage'
import SettingsPage from './pages/SettingsPage'

const AppContent = () => {
  useWindowViewModelAtom()

  return (
    <Box sx={{ height: '100vh', overflow: 'hidden' }}>
      <AlarmView />

      <HeaderProvider>
        <TheHeader />
        <TheDrawer />
      </HeaderProvider>

      <Box component="main" sx={{ height: 1, overflow: 'auto' }}>
        <Routes>
          <Route index element={<IndexPage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/settings" element={<SettingsPage />} />
        </Routes>
      </Box>
    </Box>
  )
}

export default AppContent
