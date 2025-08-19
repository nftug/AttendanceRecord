export type WindowCommandEnvelope =
  | {
      command: 'messageBox'
      payload: {
        title?: string
        message: string
        buttons?: 'Ok' | 'OkCancel' | 'YesNo' | 'YesNoCancel'
        icon?: 'Info' | 'Warning' | 'Error' | 'Question'
      }
    }
  | {
      command: 'setMinimized'
      payload: boolean
    }
  | {
      command: 'sendNotification'
      payload: {
        title: string
        message: string
      }
    }

export type WindowEventEnvelope =
  | {
      event: 'receive:messageBox'
      payload: 'Ok' | 'Cancel' | 'Yes' | 'No'
      commandName: 'messageBox'
    }
  | {
      event: 'receive:setMinimized'
      commandName: 'setMinimized'
    }
  | {
      event: 'receive:sendNotification'
      commandName: 'sendNotification'
    }
