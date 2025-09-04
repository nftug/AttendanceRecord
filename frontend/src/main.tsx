import { initializeIpcHandler } from '@/lib/api/handlers/ipcHandler.ts'
import { createTheme, CssBaseline, ThemeProvider } from '@mui/material'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import dayjs from 'dayjs'
import 'dayjs/locale/ja'
import { SnackbarProvider } from 'notistack'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import App from './App.tsx'

initializeIpcHandler(window.external)

dayjs.locale('ja')

const theme = createTheme({ colorSchemes: { dark: true } })
const queryClient = new QueryClient()

createRoot(document.getElementById('root')!).render(
  <StrictMode>
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
              <App />
            </LocalizationProvider>
          </SnackbarProvider>
        </ThemeProvider>
      </QueryClientProvider>
    </BrowserRouter>
  </StrictMode>
)
