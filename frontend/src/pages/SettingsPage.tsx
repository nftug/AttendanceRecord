import { Container, Stack, Typography, useColorScheme } from '@mui/material'
import AppConfigSettingsSection from '../features/settings/components/AppConfigSettingsSection'
import ThemeSettingsSection from '../features/settings/components/ThemeSettingsSection'

const SettingsPage: React.FC = () => {
  const { mode, setMode } = useColorScheme()

  return (
    <Container sx={{ mt: 5, mb: 6 }}>
      <Stack spacing={4}>
        <Typography variant="h4">設定</Typography>
        <ThemeSettingsSection mode={mode} onChangeMode={setMode} />
        <AppConfigSettingsSection />
      </Stack>
    </Container>
  )
}

export default SettingsPage
