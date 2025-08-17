import { createTheme, CssBaseline, ThemeProvider } from '@mui/material'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import 'dayjs/locale/ja'
import { SnackbarProvider } from 'notistack'
import { BrowserRouter } from 'react-router-dom'
import AppContent from './AppContent'

const theme = createTheme({ colorSchemes: { dark: true } })
const queryClient = new QueryClient()

const App = () => {
  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <SnackbarProvider
            anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
            preventDuplicate
            disableWindowBlurListener
          >
            <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="ja">
              <AppContent />
            </LocalizationProvider>
          </SnackbarProvider>
        </ThemeProvider>
      </QueryClientProvider>
    </BrowserRouter>
  )
}

export default App
