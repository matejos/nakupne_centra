using nakupne_centra.DataModel;
using nakupne_centra.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace nakupne_centra
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel viewModel = new MainViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }

        private void CentreListView_ItemClicked(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(CentrePage), e.ClickedItem);
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
            viewModel.RefreshFilteredData();
        }
    }
}
