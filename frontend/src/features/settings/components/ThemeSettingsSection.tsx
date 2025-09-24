import {
  FormControl,
  FormControlLabel,
  Paper,
  Radio,
  RadioGroup,
  Stack,
  Typography
} from '@mui/material'

export type ThemeSettingsSectionProps = {
  mode: 'system' | 'light' | 'dark' | undefined
  onChangeMode: (mode: 'system' | 'light' | 'dark') => void
}

const ThemeSettingsSection: React.FC<ThemeSettingsSectionProps> = ({ mode, onChangeMode }) => (
  <Paper variant="outlined" sx={{ p: 3 }}>
    <Stack spacing={2}>
      <Typography variant="subtitle1" fontWeight="bold">
        テーマ設定
      </Typography>

      <FormControl>
        {mode && (
          <RadioGroup
            row
            value={mode}
            onChange={(event) => onChangeMode(event.target.value as 'system' | 'light' | 'dark')}
          >
            <FormControlLabel value="system" control={<Radio />} label="システム" />
            <FormControlLabel value="light" control={<Radio />} label="ライト" />
            <FormControlLabel value="dark" control={<Radio />} label="ダーク" />
          </RadioGroup>
        )}
      </FormControl>
    </Stack>
  </Paper>
)

export default ThemeSettingsSection
