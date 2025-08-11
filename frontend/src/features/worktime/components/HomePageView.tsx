import { flexCenterStyle } from '@/lib/layout/constants/styles'
import CoffeeIcon from '@mui/icons-material/Coffee'
import PlayCircleIcon from '@mui/icons-material/PlayCircle'
import StopCircleIcon from '@mui/icons-material/StopCircle'
import { Box, Button, Paper, Stack, Typography } from '@mui/material'
import dayjs from 'dayjs'
import { HomePageViewModel, useHomePageViewModel } from '../atoms/homePageViewModel'

const HomePageView = () => {
  const viewModel = useHomePageViewModel()
  return viewModel && <HomePageViewInternal {...viewModel} />
}

const HomePageViewInternal = ({ state, invoke }: HomePageViewModel) => {
  const workIcon = state.isActive ? <StopCircleIcon /> : <PlayCircleIcon />
  const workLabel = state.isActive ? '退勤' : '出勤'
  const restLabel = state.isResting ? '休憩終了' : '休憩開始'
  const formattedCurrentDateTime = dayjs(state.currentDateTime).format('HH:mm:ss')

  return (
    <Stack sx={{ ...flexCenterStyle, height: 1, p: 4 }} spacing={3}>
      {/* 時計表示 */}
      <Box sx={{ display: 'flex', alignItems: 'center', height: 0.4 }}>
        <Typography align="center" sx={{ fontSize: '4.5rem' }}>
          {formattedCurrentDateTime}
        </Typography>
      </Box>

      <Stack sx={{ width: 1, height: 0.4 }} spacing={3}>
        {/* トグルボタン */}
        <Stack direction="row" spacing={2} justifyContent="center">
          <Button
            variant="contained"
            color="primary"
            startIcon={workIcon}
            onClick={() => invoke('toggleWork')}
            fullWidth
            size="large"
          >
            {workLabel}
          </Button>
          <Button
            variant="contained"
            color="success"
            startIcon={<CoffeeIcon />}
            onClick={() => invoke('toggleRest')}
            fullWidth
            disabled={!state.isActive}
            size="large"
          >
            {restLabel}
          </Button>
        </Stack>

        {/* 勤務・休憩・残業情報 */}
        <Paper variant="outlined" sx={{ px: 3, display: 'flex', alignItems: 'center', height: 1 }}>
          <Stack spacing={1} sx={{ width: 1 }}>
            <Stack direction="row" alignItems="center">
              <Typography fontWeight="bold" sx={{ width: 200 }}>
                勤務時間
              </Typography>
              <Typography sx={{ flex: 1 }}>{state.workTime}</Typography>
            </Stack>
            <Stack direction="row" alignItems="center">
              <Typography fontWeight="bold" sx={{ width: 200 }}>
                休憩時間
              </Typography>
              <Typography sx={{ flex: 1 }}>{state.restTime}</Typography>
            </Stack>
            <Stack direction="row" alignItems="center">
              <Typography fontWeight="bold" sx={{ width: 200 }}>
                本日の残業時間
              </Typography>
              <Typography sx={{ flex: 1 }}>{state.overtime}</Typography>
            </Stack>
            <Stack direction="row" alignItems="center">
              <Typography fontWeight="bold" sx={{ width: 200 }}>
                今月の残業時間
              </Typography>
              <Typography sx={{ flex: 1 }}>{state.overtimeMonthly}</Typography>
            </Stack>
          </Stack>
        </Paper>
      </Stack>
    </Stack>
  )
}

export default HomePageView
