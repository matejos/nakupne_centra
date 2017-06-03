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

        private void ScrollViewer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
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
                        sv.ChangeView((p.X + sv.HorizontalOffset) / sv.ZoomFactor * 1.5 - Window.Current.Bounds.Width / 2,
                                      (p.Y + sv.VerticalOffset) / sv.ZoomFactor * 1.5 - Window.Current.Bounds.Height * 0.7, (float)1.5);
                    }
                    else
                    {
                        sv.ChangeView((p.X + sv.HorizontalOffset) / sv.ZoomFactor * 0.5 - Window.Current.Bounds.Width / 2,
                                      (p.Y + sv.VerticalOffset) / sv.ZoomFactor * 0.5 - Window.Current.Bounds.Height * 0.7, (float)0.5);
                    }
                });
            }
            , period);
        }
    }
}
