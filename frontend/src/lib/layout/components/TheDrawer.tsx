import { CalendarMonth, Home, Info, Settings } from '@mui/icons-material'
import {
  Box,
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText
} from '@mui/material'
import { useContext } from 'react'
import { Link } from 'react-router-dom'
import { DrawerContext, DrawerDispatchContext } from '../contexts/DrawerContext'

type DrawerItem = {
  name: string
  href: string
  icon?: React.JSX.Element
}

const TheDrawer = () => {
  const [drawerOpened, setDrawerOpened] = [
    useContext(DrawerContext),
    useContext(DrawerDispatchContext)
  ]

  const menuItems: DrawerItem[] = [
    { name: 'Home', href: '/', icon: <Home /> },
    { name: 'History', href: '/history', icon: <CalendarMonth /> },
    { name: 'Settings', href: '/settings', icon: <Settings /> },
    { name: 'About', href: '/about', icon: <Info /> }
  ]

  return (
    <Drawer open={drawerOpened} onClose={() => setDrawerOpened(false)}>
      <Box sx={{ width: 250 }} role="presentation" onClick={() => setDrawerOpened(false)}>
        <List>
          {menuItems.map((item) => (
            <ListItem key={item.name} disablePadding>
              <ListItemButton component={Link} to={item.href}>
                <ListItemIcon children={item.icon} />
                <ListItemText primary={item.name} />
              </ListItemButton>
            </ListItem>
          ))}
        </List>
      </Box>
    </Drawer>
  )
}

export default TheDrawer
