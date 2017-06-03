using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using nakupne_centra.DataModel;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;




// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace nakupne_centra.ViewModel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private MapViewModel viewModel;

        public MapPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MapCentreStore css = e.Parameter as MapCentreStore;
            viewModel = new MapViewModel(css.Centre, css.Store);
            DataContext = viewModel;


            MapScrollViewer.ChangeView(null, null, (float)0.4);

            if (viewModel.Floors == 1)
            {
                FloorSlider.Visibility = Visibility.Collapsed;
            } else
            {
                FloorSlider.Visibility = Visibility.Visible;
                FloorSlider.Maximum = viewModel.MaxFloor;
                FloorSlider.Minimum = viewModel.MinFloor;

                if (viewModel.SelectedStore != null)
                {
                    switch (viewModel.SelectedStore.Floor)
                    {
                        case "0":
                            FloorSlider.Value = 0;
                            break;
                        case "1":
                            FloorSlider.Value = 1;
                            break;
                    };
                    TimeSpan period = TimeSpan.FromMilliseconds(200);

                    Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
                    {
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            MapScrollViewer.ChangeView(viewModel.SelectedStore.PositionX / MapScrollViewer.ZoomFactor - Window.Current.Bounds.Width / 2,
                                      viewModel.SelectedStore.PositionY / MapScrollViewer.ZoomFactor - Window.Current.Bounds.Height * 0.7, null, false);
                        });
                    }
                    , period);
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
        }

        private void ScrollViewer_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            if (sv == null) return;
            Point p = e.GetPosition(sv);

            TimeSpan period = TimeSpan.FromMilliseconds(200);

            Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (sv.ZoomFactor <= 1)
                    {
                        sv.ChangeView((p.X + sv.HorizontalOffset) / sv.ZoomFactor * 1.5 - Window.Current.Bounds.Width / 2,
                                      (p.Y + sv.VerticalOffset) / sv.ZoomFactor * 1.5 - Window.Current.Bounds.Height * 0.7, (float)1.5, false);
                    }
                    else
                    {
                        sv.ChangeView((p.X + sv.HorizontalOffset) / sv.ZoomFactor * 0.5 - Window.Current.Bounds.Width / 2,
                                      (p.Y + sv.VerticalOffset) / sv.ZoomFactor * 0.5 - Window.Current.Bounds.Height * 0.7, (float)0.5, false);
                    }
                });
            }
            , period);
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                double X = MapScrollViewer.HorizontalOffset;
                double Y = MapScrollViewer.VerticalOffset;
                if (slider.Value == 0)
                {
                    Map.Source = viewModel.Map0;
                    if (viewModel.SelectedStore != null)
                    {
                        switch (viewModel.SelectedStore.Floor)
                        {
                            case "1":
                                StorePosition.Visibility = Visibility.Collapsed;
                                break;
                            default:
                                StorePosition.Visibility = Visibility.Visible;
                                break;
                        };
                    }
                } else
                {
                    Map.Source = viewModel.Map1;
                    if (viewModel.SelectedStore != null)
                    {
                        switch (viewModel.SelectedStore.Floor)
                        {
                            case "0":
                                StorePosition.Visibility = Visibility.Collapsed;
                                break;
                            default:
                                StorePosition.Visibility = Visibility.Visible;
                                break;
                        };
                    }
                }
                TimeSpan period = TimeSpan.FromMilliseconds(200);

                Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        MapScrollViewer.ChangeView(X, Y, null, true);
                    });
                }
                , period);
            }
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

        private void Map_ImageOpened(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            if (img.Source == DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor0)
            {
                DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor0Height = (img.Source as BitmapImage).PixelHeight;
            }
            else if (img.Source == DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor1)
            {
                DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor1Height = (img.Source as BitmapImage).PixelHeight;
            }
        }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = (args.SelectedItem as Store).Name;
            viewModel.SelectedStore = (args.SelectedItem as Store);
            viewModel.RefreshSelectedStore();
        }
    }
}
