import { useProvideHomePageViewModel } from '@/features/worktime/atoms/homePageViewModel'
import HomePageView from '@/features/worktime/components/HomePageView'

const IndexPage = () => {
  useProvideHomePageViewModel()
  return <HomePageView />
}

export default IndexPage
