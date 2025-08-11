import { Theme } from '@emotion/react'
import { SxProps } from '@mui/material'

export const overflowEllipsisStyle: SxProps<Theme> = {
  overflow: 'hidden',
  textOverflow: 'ellipsis',
  whiteSpace: 'nowrap',
  maxWidth: 1
} as const

export const flexCenterStyle: SxProps<Theme> = {
  display: 'flex',
  justifyContent: 'center',
  alignItems: 'center',
  minHeight: 1
} as const
