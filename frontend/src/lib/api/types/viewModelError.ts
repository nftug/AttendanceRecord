import { ViewId } from './apiTypes'
import { ViewModelErrorEventResult } from './appTypes'

export class ViewModelError extends Error {
  readonly viewId: ViewId
  readonly details?: string

  constructor(viewId: ViewId, { message, details }: ViewModelErrorEventResult) {
    super(message)

    if (Error.captureStackTrace) {
      Error.captureStackTrace(this, ViewModelError)
    }

    this.name = 'ViewModelError'
    this.viewId = viewId
    this.details = details
  }
}
