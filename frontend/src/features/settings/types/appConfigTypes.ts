export type AppConfigResponseDto = {
  standardWorkMinutes: number
  residentNotificationEnabled: boolean
  workEndAlarm: {
    isEnabled: boolean
    remainingMinutes: number
    snoozeMinutes: number
  }
  restStartAlarm: {
    isEnabled: boolean
    elapsedMinutes: number
    snoozeMinutes: number
  }
  statusFormat: {
    statusFormat: string
    timeSpanFormat: string
  }
}

export type AppConfigSaveRequestDto = {
  standardWorkMinutes: number
  residentNotificationEnabled: boolean
  workEndAlarm: {
    isEnabled: boolean
    remainingMinutes: number
    snoozeMinutes: number
  }
  restStartAlarm: {
    isEnabled: boolean
    elapsedMinutes: number
    snoozeMinutes: number
  }
  statusFormat: {
    statusFormat: string
    timeSpanFormat: string
  }
}

export type AppConfigCommands =
  | {
      command: 'getAppConfig'
    }
  | {
      command: 'saveAppConfig'
      payload: AppConfigSaveRequestDto
    }

export type AppConfigEvents =
  | {
      event: 'receive:getAppConfig'
      commandName: 'getAppConfig'
      payload: AppConfigResponseDto
    }
  | {
      event: 'receive:saveAppConfig'
      commandName: 'saveAppConfig'
    }
