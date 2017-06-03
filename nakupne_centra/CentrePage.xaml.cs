using nakupne_centra.DataModel;
using nakupne_centra.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace nakupne_centra
{
    public sealed partial class CentrePage : Page
    {
        private CentreViewModel viewModel;

        public CentrePage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        private void centresStoreSearch_QueryChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            CentreStoreSearch css = new CentreStoreSearch(viewModel.Centre, sender.Text, null, true);
            this.Frame.Navigate(typeof(StoresList), css);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel = new CentreViewModel(e.Parameter as Centre);
            DataContext = viewModel;
            (App.Current as App).CategoryExpanded = new System.Collections.Generic.Dictionary<string, bool>();
        }

        private void ButtonStores_Click(object sender, RoutedEventArgs e)
        {
            CentreStoreSearch css = new CentreStoreSearch(viewModel.Centre);
            this.Frame.Navigate(typeof(StoresList), css);
        }

        public void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void MapButtonClick(object sender, RoutedEventArgs e)
        {
            MapCentreStore css = new MapCentreStore(viewModel.Centre, null);
            this.Frame.Navigate(typeof(MapPage), css);
        }
    }
}
