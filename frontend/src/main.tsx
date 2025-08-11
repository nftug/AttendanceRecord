import { initializeIpcHandler } from '@/lib/api/handlers/ipcHandler.ts'
import dayjs from 'dayjs'
import duration from 'dayjs/plugin/duration'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import App from './App.tsx'

initializeIpcHandler(window.external)
dayjs.extend(duration)

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>
)
