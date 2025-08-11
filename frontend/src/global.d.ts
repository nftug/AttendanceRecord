declare global {
  interface External {
    sendMessage: (message: string) => void
    receiveMessage: (callback: (message: string) => void) => void
  }

  interface Window {
    external: External
  }
}

export {}
