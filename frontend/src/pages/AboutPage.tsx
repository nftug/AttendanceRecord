import { flexCenterStyle } from '@/lib/layout/constants/styles'
import { Box, Typography } from '@mui/material'

const AboutPage = () => {
  return (
    <Box sx={flexCenterStyle}>
      <Typography variant="h2" color="textSecondary">
        {import.meta.env.VITE_APP_NAME}
      </Typography>
    </Box>
  )
}

export default AboutPage
