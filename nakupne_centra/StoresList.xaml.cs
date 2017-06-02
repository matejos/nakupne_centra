using nakupne_centra.DataModel;
using nakupne_centra.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace nakupne_centra
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoresList : Page
    {
        private StoresListViewModel viewModel;
        private bool focusSearchBar;

        public StoresList()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            this.Loaded += Page_Loaded;
            StoresListView.SelectionChanged += StoresListView_SelectionChanged;
            SystemNavigationManager.GetForCurrentView().BackRequested += StoresList_BackRequested;
            if ((App.Current as App).PreferSortingByType)
                SortByTypeButton_Click(null, null);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CentreStoreSearch css = e.Parameter as CentreStoreSearch;
            viewModel = new StoresListViewModel(css.Centre);
            viewModel.NameFilter = css.Query;
            DataContext = viewModel;
            viewModel.SelectedStore = css.Store;
            focusSearchBar = css.FocusSearchBar;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (focusSearchBar)
                SearchBox.Focus(FocusState.Programmatic);
            if (viewModel.SelectedStore != null)
                StoresListView.SelectedItem = viewModel.SelectedStore;
            UpdateActiveViewState(Window.Current.Bounds.Width);
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            UpdateActiveViewState(e.Size.Width);
        }
        private void UpdateActiveViewState(double windowWidth)
        {
            if (windowWidth > 720)
            {
                EnsureViewStateActive("FullView", WidthDisplayStates);
            }
            else
            {
                if (StoresListView.SelectedItem == null)
                {
                    EnsureViewStateActive("ListView", WidthDisplayStates);
                    MainSplitView.OpenPaneLength = windowWidth;
                }
                else
                {
                    EnsureViewStateActive("DetailView", WidthDisplayStates);
                }
            }
        }
        private void EnsureViewStateActive(string viewState, VisualStateGroup stateGroup)
        {
            if (stateGroup.CurrentState == null || stateGroup.CurrentState.Name != viewState)
            {
                VisualStateManager.GoToState(this, viewState, false);
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void StoresListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WidthDisplayStates.CurrentState != null && WidthDisplayStates.CurrentState.Name == "ListView")
            {
                if (StoresListView.SelectedItem != null)
                {
                    
                    VisualStateManager.GoToState(this, "DetailView", false);
                }
            }
            else if (WidthDisplayStates.CurrentState != null && WidthDisplayStates.CurrentState.Name == "DetailView")
            {
                if (StoresListView.SelectedItem == null)
                {
                    VisualStateManager.GoToState(this, "ListView", false);
                }
            }
            if (StoresListView.SelectedItem != null)
                viewModel.SelectedStore = StoresListView.SelectedItem as Store;
        }

        private void StoresList_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (WidthDisplayStates.CurrentState.Name == "DetailView")
            {
                StoresListView.SelectedItem = null;
                MainSplitView.OpenPaneLength = Window.Current.Bounds.Width;
                e.Handled = true;
            }
            else
            { 
                e.Handled = true;
                (Window.Current.Content as Frame).GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= StoresList_BackRequested;
        }

        private void centresStoreSearch_QueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
        }

        private void SortByAlphabetButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "ByAlphabet", false);
            (App.Current as App).PreferSortingByType = false;
        }

        private void SortByTypeButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "ByType", false);
            (App.Current as App).PreferSortingByType = true;
        }
    }
}
