using nakupne_centra.DataModel;
using nakupne_centra.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace nakupne_centra
{
    public sealed partial class StoresList : Page
    {
        private StoresListViewModel viewModel;
        private bool focusSearchBar;
        private ObservableCollection<ExpandPanel> panels = new ObservableCollection<ExpandPanel>();

        public StoresList()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            this.Loaded += Page_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += StoresList_BackRequested;
            OpeningHoursPanel.Visibility = Visibility.Collapsed;
            StorePosition.Visibility = Visibility.Collapsed;
            if ((App.Current as App).PreferSortingByType)
                SortByTypeButton_Click(null, null);
            else
                SortByAlphabetButton_Click(null, null);
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
                    MainSplitView.OpenPaneLength = 0;
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

        private void StoresListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StoreListView_SelectionChangedOrItemClicked(sender);
        }

        private void StoresListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            StoreListView_SelectionChangedOrItemClicked(sender);
        }

        private void StoreListView_SelectionChangedOrItemClicked(object sender)
        {
            OpeningHoursPanel.Visibility = Visibility.Visible;
            StorePosition.Visibility = Visibility.Visible;
            if (WidthDisplayStates.CurrentState != null && WidthDisplayStates.CurrentState.Name == "ListView")
            {
                if ((sender as ListView).SelectedItem != null)
                {
                    VisualStateManager.GoToState(this, "DetailView", false);
                    MainSplitView.OpenPaneLength = 0;
                }
            }
            else if (WidthDisplayStates.CurrentState != null && WidthDisplayStates.CurrentState.Name == "DetailView")
            {
                if ((sender as ListView).SelectedItem == null)
                {
                    VisualStateManager.GoToState(this, "ListView", false);
                    MainSplitView.OpenPaneLength = Window.Current.Bounds.Width;
                }
            }
            if ((sender as ListView).SelectedItem != null)
                viewModel.SelectedStore = (sender as ListView).SelectedItem as Store;
        }

        private void CategoryStoresListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategoryStoreListView_SelectionChangedOrItemClicked(sender);
        }

        private void CategoryStoresListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            CategoryStoreListView_SelectionChangedOrItemClicked(sender);
        }

        private void CategoryStoreListView_SelectionChangedOrItemClicked(object sender)
        {
            if ((sender as ListView).SelectedItem != null)
            {
                var _Children = AllChildren(StoresByCategoryListView);
                IEnumerable<ListView> lists = _Children.OfType<ListView>()
                    .Where(x => x.Name.Equals("CategoryStoresList") && x != sender);

                foreach (var list in lists)
                    list.SelectedItem = null;
            }
            StoreListView_SelectionChangedOrItemClicked(sender);
        }

        public List<Control> AllChildren(DependencyObject parent)
        {
            var _List = new List<Control>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is Control)
                    _List.Add(_Child as Control);
                _List.AddRange(AllChildren(_Child));
            }
            return _List;
        }

        private void StoresList_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (WidthDisplayStates.CurrentState.Name == "DetailView")
            {
                EnsureViewStateActive("ListView", WidthDisplayStates);
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

        private void PanelLoaded(object sender, RoutedEventArgs e)
        {
            ExpandPanel panel = (sender as ListView).Parent as ExpandPanel;
            panel.PanelLoaded();
            panels.Add(panel);
            if (SearchBox.Text != "")
                panel.ToggleExpand(true);
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            foreach (ExpandPanel panel in panels)
            {
                if (sender.Text != "")
                    panel.ToggleExpand(true);
                else
                    panel.ReturnExpandState();
            }
        }
    }
}
