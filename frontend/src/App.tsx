import { Box } from '@mui/material'
import { Route, Routes } from 'react-router-dom'
import { useProvideWindowViewModel } from './features/window/atoms/windowViewModel'
import AlarmView from './features/worktime/components/AlarmView'
import HomePageView from './features/worktime/components/HomePageView'
import { HeaderProvider } from './lib/layout/components/HeaderContext'
import TheDrawer from './lib/layout/components/TheDrawer'
import TheHeader from './lib/layout/components/TheHeader'
import AboutPage from './pages/AboutPage'
import HistoryPage from './pages/HistoryPage'
import SettingsPage from './pages/SettingsPage'

const App = () => {
  useProvideWindowViewModel()

  return (
    <Box sx={{ height: '100vh', display: 'flex', flexDirection: 'column' }}>
      <AlarmView />

      <HeaderProvider>
        <TheHeader />
        <TheDrawer />
      </HeaderProvider>

      {/* main エリアはヘッダーを除いた領域を占有し、内部スクロールはここで処理する */}
      <Box component="main" sx={{ flex: '1 1 0', minHeight: 0, overflow: 'auto' }}>
        <Routes>
          <Route index element={<HomePageView />} />
          <Route path="/history" element={<HistoryPage />} />
          <Route path="/about" element={<AboutPage />} />
          <Route path="/settings" element={<SettingsPage />} />
        </Routes>
      </Box>
    </Box>
  )
}

export default App
