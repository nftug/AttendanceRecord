import { initializeIpcHandler } from '@/lib/api/handlers/ipcHandler.ts'
import dayjs from 'dayjs'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'

initializeIpcHandler(window.external)

dayjs.locale('ja')

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>
)
