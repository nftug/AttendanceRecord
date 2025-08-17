import {
  CalendarMonth,
  CalendarToday,
  Delete,
  Home,
  NavigateBefore,
  NavigateNext,
  Save,
  Today
} from '@mui/icons-material'
import {
  Box,
  Divider,
  IconButton,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Paper,
  Toolbar,
  Typography,
  useTheme
} from '@mui/material'
import { useQuery } from '@tanstack/react-query'
import dayjs from 'dayjs'
import { useState } from 'react'
import Split from 'react-split'
import { HistoryPageViewModel, useHistoryPageViewModel } from '../atoms/historyPageViewModel'

const HistoryPageView = () => {
  const viewModel = useHistoryPageViewModel()
  return viewModel && <HistoryPageViewInternal {...viewModel} />
}

const HistoryPageViewInternal = ({ invoke, isInitialized }: HistoryPageViewModel) => {
  const [yearAndMonth, setYearAndMonth] = useState(dayjs())

  const { data: listData, isLoading: isLoadingList } = useQuery({
    queryKey: ['history', yearAndMonth],
    queryFn: async () =>
      await invoke('getWorkRecordList', {
        year: yearAndMonth.year(),
        month: yearAndMonth.month() + 1
      }),
    enabled: isInitialized
  })

  const theme = useTheme()

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', height: '100%', minHeight: 0 }}>
      <Paper
        elevation={0}
        sx={{
          alignItems: 'center',
          border: '1px solid',
          borderColor: theme.palette.divider
        }}
      >
        <Toolbar>
          <IconButton onClick={() => setYearAndMonth((prev) => prev.add(-1, 'month'))}>
            <NavigateBefore />
          </IconButton>
          <IconButton onClick={() => setYearAndMonth((prev) => prev.add(1, 'month'))}>
            <NavigateNext />
          </IconButton>
          <IconButton onClick={() => setYearAndMonth(dayjs())}>
            <Home />
          </IconButton>
          <IconButton onClick={() => {}}>
            <Today />
          </IconButton>

          <Divider orientation="vertical" flexItem sx={{ mx: 2 }} />

          <IconButton onClick={() => {}}>
            <Save />
          </IconButton>
          <IconButton onClick={() => {}}>
            <Delete />
          </IconButton>

          <Box sx={{ flexGrow: 1 }} />

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <CalendarMonth />
            <Typography variant="body1">
              {isLoadingList
                ? 'Loading...'
                : `${yearAndMonth.year()}年 ${(yearAndMonth.month() + 1).toString().padStart(2, '0')}月`}
            </Typography>
          </Box>
        </Toolbar>
      </Paper>

      <Split
        sizes={[25, 75]}
        minSize={100}
        direction="horizontal"
        style={{ display: 'flex', flexGrow: 1, minHeight: 0 }}
        gutterSize={16}
        gutterStyle={() => ({
          background: theme.palette.divider,
          width: '6px',
          cursor: 'col-resize'
        })}
      >
        <Paper sx={{ overflow: 'auto', height: '100%' }}>
          <List sx={{ p: 1 }}>
            {listData?.workRecords.map((item) => (
              <ListItem key={item.id} sx={{ p: 0.5 }}>
                <ListItemButton>
                  <ListItemIcon>
                    <CalendarToday />
                  </ListItemIcon>
                  <ListItemText primary={dayjs(item.date).format('YYYY/MM/DD')} />
                </ListItemButton>
              </ListItem>
            ))}
          </List>
        </Paper>

        <Box sx={{ overflow: 'auto', height: '100%' }}></Box>
      </Split>
    </Box>
  )
}

export default HistoryPageView
