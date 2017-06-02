using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using nakupne_centra.DataModel;
using Windows.Foundation;




// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace nakupne_centra.ViewModel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private CentreViewModel viewModel;

        public MapPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            viewModel = new CentreViewModel(e.Parameter as Centre);
            DataContext = viewModel;
            (App.Current as App).CategoryExpanded = new System.Collections.Generic.Dictionary<string, bool>();
        }

        private async void scrollViewer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            if (sv == null) return;
            Point p = e.GetPosition(sv);

            TimeSpan period = TimeSpan.FromMilliseconds(10);

            Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                if (sv.ZoomFactor <= 1)
                {
                    var k = sv.ChangeView(p.X + sv.HorizontalOffset * 2, p.Y + sv.VerticalOffset * 2, 2);
                    }
                    else
                    {
                        sv.ChangeView(sv.HorizontalOffset / 2 - p.X, sv.VerticalOffset / 2 - p.Y, 1);
                    }
                });
            }
            , period);
        }

        /*private void Image_PointerWheelChanged_1(object sender, PointerRoutedEventArgs e)
        {
            double newvalue = e.GetCurrentPoint(sender as UIElement).Properties.MouseWheelDelta;
            if (newvalue > 0)
            {
                MapScrollViewer.ZoomToFactor(MapScrollViewer.ZoomFactor*(float)1.2);
            }
            if (newvalue < 0)
            {
                MapScrollViewer.ZoomToFactor(MapScrollViewer.ZoomFactor / (float)1.2);
            }
        }*/
    }
}
