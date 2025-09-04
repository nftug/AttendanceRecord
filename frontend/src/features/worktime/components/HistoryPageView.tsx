import { useWindowViewModel } from '@/features/window/atoms/windowViewModel'
import DateSelectModal from '@/lib/ui/form/components/DateSelectModal'
import { formatDate, formatDateTime, toDateTimeString } from '@/lib/utils/dayjsUtils'
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
import dayjs, { Dayjs } from 'dayjs'
import { useEffect, useRef, useState } from 'react'
import Split from 'react-split'
import { HistoryPageViewModel, useHistoryPageViewModel } from '../atoms/historyPageViewModel'
import { useWorkRecordListQuery } from '../hooks/historyPageQueries'
import useAskDeleteWorkRecord from '../hooks/useAskDeleteWorkRecord'
import HistoryItemView, { HistoryItemViewHandle } from './HistoryItemView'

const HistoryPageView = () => {
  const viewModel = useHistoryPageViewModel()
  return viewModel && <HistoryPageViewInternal {...viewModel} />
}

const HistoryPageViewInternal = ({ invoke, isInitialized }: HistoryPageViewModel) => {
  const [monthDate, setMonthDate] = useState(dayjs())
  const [selectedDate, setSelectedDate] = useState<Dayjs | null>(dayjs())
  const [pendingAutoSelect, setPendingAutoSelect] = useState(false)
  const [isFormDirty, setIsFormDirty] = useState(false)

  const { data: listData, isLoading: isLoadingList } = useWorkRecordListQuery({
    options: { recordedMonthDate: toDateTimeString(monthDate) },
    viewModel: { invoke, isInitialized }
  })

  const selectedItemId = selectedDate
    ? (listData?.workRecords.find((x) => dayjs(x.date).isSame(selectedDate, 'day'))?.id ?? null)
    : null

  const theme = useTheme()
  const { invoke: invokeWindow } = useWindowViewModel()

  const confirmDiscard = async () => {
    if (!isFormDirty) return true
    const result = await invokeWindow({
      command: 'messageBox',
      payload: {
        title: '確認',
        message: '保存されていない変更があります。このまま移動しますか？',
        buttons: 'OkCancel',
        icon: 'Warning'
      }
    })
    return result === 'Ok'
  }

  const handleNavigate = async (direction: 'prev' | 'next') => {
    // 子コンポーネントに未保存の変更を破棄しても良いか確認する
    if (!(await confirmDiscard())) return

    setPendingAutoSelect(true)
    setMonthDate((prev) => prev.add(direction === 'prev' ? -1 : 1, 'month'))
  }

  // handleNavigate が使われたとき、最後の項目を自動選択する
  useEffect(() => {
    if (!listData || !pendingAutoSelect) return

    setPendingAutoSelect(false)
    const lastRecord = listData.workRecords.at(-1)
    setSelectedDate(lastRecord ? dayjs(lastRecord.date) : null)
  }, [listData, pendingAutoSelect])

  const handleDateSelect = async (date: Dayjs) => {
    if (!(await confirmDiscard())) return
    setSelectedDate(date)

    if (date.year() !== monthDate.year() || date.month() !== monthDate.month()) {
      setMonthDate(date)
    }
  }

  const handleDateModalSelect = async (initialDate: Dayjs | null) => {
    const result = await DateSelectModal.call({ initialDate })
    if (result) handleDateSelect(result)
  }

  const handleClickDeleteSelected = useAskDeleteWorkRecord({
    itemId: selectedItemId,
    viewModel: { invoke, isInitialized },
    onSuccess: () => setSelectedDate(null)
  })

  const historyItemRef = useRef<HistoryItemViewHandle | null>(null)
  const [canSubmit, setCanSubmit] = useState(false)

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
          <IconButton onClick={() => handleNavigate('prev')}>
            <NavigateBefore />
          </IconButton>
          <IconButton onClick={() => handleNavigate('next')}>
            <NavigateNext />
          </IconButton>
          <IconButton onClick={() => handleDateSelect(dayjs())}>
            <Home />
          </IconButton>
          <IconButton onClick={() => handleDateModalSelect(selectedDate)}>
            <Today />
          </IconButton>

          <Divider orientation="vertical" flexItem sx={{ mx: 2 }} />

          <IconButton
            onClick={() => historyItemRef.current?.submit()}
            disabled={!canSubmit || !selectedDate}
          >
            <Save />
          </IconButton>
          <IconButton onClick={handleClickDeleteSelected} disabled={!selectedItemId}>
            <Delete />
          </IconButton>

          <Box sx={{ flexGrow: 1 }} />

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <CalendarMonth />
            <Typography variant="body1">
              {isLoadingList ? 'Loading...' : formatDateTime(monthDate, 'YYYY年 MM月')}
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
                <ListItemButton
                  onClick={() => handleDateSelect(dayjs(item.date))}
                  selected={selectedDate?.isSame(dayjs(item.date), 'day')}
                >
                  <ListItemIcon>
                    <CalendarToday />
                  </ListItemIcon>
                  <ListItemText primary={formatDate(item.date)} />
                </ListItemButton>
              </ListItem>
            ))}
          </List>
        </Paper>

        <Box sx={{ overflow: 'auto', height: '100%' }}>
          <HistoryItemView
            ref={historyItemRef}
            itemId={selectedItemId}
            date={selectedDate}
            onChangeCanSubmit={setCanSubmit}
            onChangeIsDirty={setIsFormDirty}
            {...{ invoke, isInitialized }}
          />
        </Box>
      </Split>

      <DateSelectModal.Root />
    </Box>
  )
}

export default HistoryPageView
