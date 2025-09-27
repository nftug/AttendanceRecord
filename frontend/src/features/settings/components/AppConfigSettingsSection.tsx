import {
  Box,
  Button,
  CircularProgress,
  FormControlLabel,
  Paper,
  Stack,
  Switch,
  TextField,
  Typography
} from '@mui/material'
import { Controller } from 'react-hook-form'
import useAppConfigSettings from '../hooks/useAppConfigSettings'

const AppConfigSettingsSection: React.FC = () => {
  const {
    query: { data, isFetching },
    mutation,
    control,
    register,
    formState: { isDirty, isValid, errors },
    watch,
    onSubmit,
    handleReset
  } = useAppConfigSettings()

  return (
    <Stack spacing={3} component="section">
      <Stack component="form" spacing={3} onSubmit={onSubmit} noValidate>
        <Paper variant="outlined" sx={{ p: 3 }}>
          <Stack spacing={2}>
            <Typography variant="subtitle1" fontWeight="bold">
              基本設定
            </Typography>
            <TextField
              label="標準勤務時間 (分)"
              type="number"
              slotProps={{ htmlInput: { min: 0 } }}
              error={Boolean(errors.standardWorkMinutes)}
              helperText={
                errors.standardWorkMinutes?.message ?? '1日の標準勤務時間を分で指定します。'
              }
              {...register('standardWorkMinutes')}
            />
          </Stack>
        </Paper>

        <Paper variant="outlined" sx={{ p: 3 }}>
          <Stack spacing={2}>
            <Typography variant="subtitle1" fontWeight="bold">
              勤務終了アラーム
            </Typography>

            <Controller
              name="workEndAlarm.isEnabled"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      onChange={(event) => field.onChange(event.target.checked)}
                    />
                  }
                  label="アラームを有効にする"
                />
              )}
            />

            <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
              <TextField
                label="残り時間 (分)"
                type="number"
                fullWidth
                slotProps={{ htmlInput: { min: 0 } }}
                error={Boolean(errors.workEndAlarm?.remainingMinutes)}
                helperText={errors.workEndAlarm?.remainingMinutes?.message}
                disabled={!watch('workEndAlarm.isEnabled')}
                {...register('workEndAlarm.remainingMinutes')}
              />
              <TextField
                label="スヌーズ (分)"
                type="number"
                fullWidth
                slotProps={{ htmlInput: { min: 1 } }}
                error={Boolean(errors.workEndAlarm?.snoozeMinutes)}
                helperText={errors.workEndAlarm?.snoozeMinutes?.message}
                disabled={!watch('workEndAlarm.isEnabled')}
                {...register('workEndAlarm.snoozeMinutes')}
              />
            </Stack>
          </Stack>
        </Paper>

        <Paper variant="outlined" sx={{ p: 3 }}>
          <Stack spacing={2}>
            <Typography variant="subtitle1" fontWeight="bold">
              休憩開始アラーム
            </Typography>

            <Controller
              name="restStartAlarm.isEnabled"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Switch
                      checked={field.value}
                      onChange={(event) => field.onChange(event.target.checked)}
                    />
                  }
                  label="アラームを有効にする"
                />
              )}
            />

            <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
              <TextField
                label="経過時間 (分)"
                type="number"
                fullWidth
                slotProps={{ htmlInput: { min: 0 } }}
                error={Boolean(errors.restStartAlarm?.elapsedMinutes)}
                helperText={errors.restStartAlarm?.elapsedMinutes?.message}
                disabled={!watch('restStartAlarm.isEnabled')}
                {...register('restStartAlarm.elapsedMinutes')}
              />
              <TextField
                label="スヌーズ (分)"
                type="number"
                fullWidth
                slotProps={{ htmlInput: { min: 1 } }}
                error={Boolean(errors.restStartAlarm?.snoozeMinutes)}
                helperText={errors.restStartAlarm?.snoozeMinutes?.message}
                disabled={!watch('restStartAlarm.isEnabled')}
                {...register('restStartAlarm.snoozeMinutes')}
              />
            </Stack>
          </Stack>
        </Paper>

        <Paper variant="outlined" sx={{ p: 3 }}>
          <Stack spacing={3}>
            <Typography variant="subtitle1" fontWeight="bold">
              ステータス表示
            </Typography>
            <TextField
              label="ステータス書式"
              fullWidth
              helperText={errors.statusFormat?.statusFormat?.message}
              multiline
              rows={4}
              error={Boolean(errors.statusFormat?.statusFormat)}
              slotProps={{ inputLabel: { shrink: true } }}
              {...register('statusFormat.statusFormat')}
            />
            <TextField
              label="時間表示書式"
              fullWidth
              helperText={errors.statusFormat?.timeSpanFormat?.message ?? '例: hh\\:mm'}
              error={Boolean(errors.statusFormat?.timeSpanFormat)}
              slotProps={{ inputLabel: { shrink: true } }}
              {...register('statusFormat.timeSpanFormat')}
            />
          </Stack>
        </Paper>

        <Box sx={{ pt: 2 }}>
          <Stack direction="row" justifyContent="flex-end" spacing={2} alignItems="center">
            <Button
              type="button"
              onClick={handleReset}
              disabled={!data || mutation.isPending || isFetching}
            >
              変更をリセット
            </Button>
            <Box display="flex" alignItems="center" gap={2}>
              {mutation.isPending && <CircularProgress size={20} />}
              <Button
                type="submit"
                variant="contained"
                disabled={!isDirty || !isValid || mutation.isPending}
              >
                保存
              </Button>
            </Box>
          </Stack>
        </Box>
      </Stack>
    </Stack>
  )
}

export default AppConfigSettingsSection
