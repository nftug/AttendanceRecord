export type WindowCommandEnvelope = {
  command: 'messageBox'
  payload: {
    title?: string
    message: string
    buttons?: 'Ok' | 'OkCancel' | 'YesNo' | 'YesNoCancel'
    icon?: 'Info' | 'Warning' | 'Error' | 'Question'
  }
}

export type WindowEventEnvelope = {
  event: 'receive:messageBox'
  payload: 'Ok' | 'Cancel' | 'Yes' | 'No'
  commandName: 'messageBox'
}
