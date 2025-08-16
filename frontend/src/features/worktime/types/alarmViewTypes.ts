export type AlarmType = 'WorkEnd' | 'RestStart'

export type AlarmResponseDto = { type: AlarmType }

export type AlarmCommands = {
  command: 'snooze'
  payload: { type: AlarmType }
}

export type AlarmEvents =
  | {
      event: 'triggered'
      payload: AlarmResponseDto
    }
  | {
      event: 'receive:snooze'
      commandName: 'snooze'
    }
