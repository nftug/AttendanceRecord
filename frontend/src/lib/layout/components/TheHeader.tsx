import MenuIcon from '@mui/icons-material/Menu'
import { AppBar, IconButton, Stack, Toolbar, Typography } from '@mui/material'
import { useContext } from 'react'
import { DrawerDispatchContext } from '../contexts/DrawerContext'
import { useActionElementRef } from '../stores/portalAtom'

const TheHeader = () => {
  const setDrawerOpened = useContext(DrawerDispatchContext)
  const actionElementRef = useActionElementRef()

  const toggleDrawer = () => {
    setDrawerOpened((x) => !x)
  }

  return (
    <AppBar position="sticky">
      <Toolbar>
        <IconButton
          size="large"
          edge="start"
          color="inherit"
          aria-label="menu"
          sx={{ mr: 2 }}
          onClick={toggleDrawer}
        >
          <MenuIcon />
        </IconButton>

        <Typography variant="h5" sx={{ flexGrow: 1 }}>
          {import.meta.env.VITE_APP_NAME}
        </Typography>

        <Stack ref={actionElementRef} direction="row" useFlexGap />
      </Toolbar>
    </AppBar>
  )
}

export default TheHeader
