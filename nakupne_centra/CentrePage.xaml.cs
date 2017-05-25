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
    public sealed partial class CentrePage : Page
    {
        object currentCentre;
        public CentrePage()
        {
            this.InitializeComponent();
        }

        private void centresStoreSearch_QueryChanged(SearchBox sender,
         SearchBoxQueryChangedEventArgs args)
        {
            this.Frame.Navigate(typeof(CentrePage), args.QueryText);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            currentCentre = e.Parameter as object;
            MainGrid.DataContext = currentCentre;
        }
    }
}
