using nakupne_centra.DataModel;
using nakupne_centra.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace nakupne_centra
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel viewModel;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void CentreListView_ItemClicked(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(CentrePage), e.ClickedItem);
        }

        private void StoreList_ItemClicked(object sender, ItemClickEventArgs e)
        {
            Store clickedStore = e.ClickedItem as Store;
            CentreStoreSearch css = new CentreStoreSearch(clickedStore.Centre, "", clickedStore);
            this.Frame.Navigate(typeof(StoresList), css);
        }

        private void CentreListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.ItemIndex % 2 == 0)
            {
                args.ItemContainer.Background = new SolidColorBrush(Windows.UI.Colors.White);
            }
            else
            {
                args.ItemContainer.Background = new SolidColorBrush(Windows.UI.Colors.AliceBlue);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel = new MainViewModel(RootGrid);
            DataContext = viewModel;
        }

        private void ButtonClickNavigate(object sender, RoutedEventArgs e)
        {
            Store store = (sender as FrameworkElement).DataContext as Store;
            MapCentreStore css = new MapCentreStore(store.Centre, store);
            this.Frame.Navigate(typeof(MapPage), css);
        }

        private void CentreStoresButtonClick(object sender, RoutedEventArgs e)
        {
            var centre = (sender as FrameworkElement).DataContext as Centre;
            CentreStoreSearch css = new CentreStoreSearch(centre, "", null, true);
            this.Frame.Navigate(typeof(StoresList), css);
        }


        private void MapButtonClick(object sender, RoutedEventArgs e)
        {
            var centre = (sender as FrameworkElement).DataContext as Centre;
            MapCentreStore css = new MapCentreStore(centre, null);
            this.Frame.Navigate(typeof(MapPage), css);
        }
    }
}
