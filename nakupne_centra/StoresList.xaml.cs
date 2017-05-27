using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace nakupne_centra
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoresList : Page
    {
        object currentCentre;
        public StoresList()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            this.Loaded += MainPage_Loaded;
            StoresListView.SelectionChanged += StoresListView_SelectionChanged;
            SystemNavigationManager.GetForCurrentView().BackRequested += StoresList_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            currentCentre = e.Parameter as object;
            MainGrid.DataContext = currentCentre;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            UpdateActiveViewState(e.Size.Width);
        }
        private void UpdateActiveViewState(double windowWidth)
        {
            if (windowWidth > 700)
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
        }        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateActiveViewState(Window.Current.Bounds.Width);
        }        private void StoresListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WidthDisplayStates.CurrentState != null && WidthDisplayStates.CurrentState.Name ==
           "ListView")
            {
                if (StoresListView.SelectedItem != null)
                {
                    VisualStateManager.GoToState(this, "DetailView", false);
                }
            }
            else if (WidthDisplayStates.CurrentState != null &&
           WidthDisplayStates.CurrentState.Name == "DetailView")
            {
                if (StoresListView.SelectedItem == null)
                {
                    VisualStateManager.GoToState(this, "ListView", false);
                }
            }
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
    }
}
