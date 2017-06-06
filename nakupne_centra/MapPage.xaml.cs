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
using Windows.Devices.Geolocation;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;
using System.Threading.Tasks;




// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace nakupne_centra.ViewModel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private MapViewModel viewModel;
        private bool pageLoaded = false;
        private double latitude;
        private double longitude;
        private Geolocator geoLocator;
        private bool geoLocatorPositionChangedAssigned = false;
        private bool selectingManualPosition = false;
        private ResourceLoader resourceLoader;

        public MapPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            resourceLoader = ResourceLoader.GetForCurrentView();
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
            }
            else
            {
                FloorSlider.Visibility = Visibility.Visible;
                FloorSlider.Maximum = viewModel.MaxFloor;
                FloorSlider.Minimum = viewModel.MinFloor;   
            }
            if (viewModel.SelectedStore != null)
            {
                ZoomOnStore(viewModel.SelectedStore);
            }
            else
            {
                ZoomOut();
            }
            pageLoaded = true;
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
                DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor0Width = (img.Source as BitmapImage).PixelWidth;
                ZoomOut();
            }
            else if (img.Source == DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor1)
            {
                DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor1Height = (img.Source as BitmapImage).PixelHeight;
            }
        }

        private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Store selectedStore = args.SelectedItem as Store;
            viewModel.NameFilter = "";
            viewModel.NameFilter = selectedStore.Name;
            Debug.WriteLine(viewModel.NameFilter);
            if (selectingManualPosition)
            {
                viewModel.LocatedStore = selectedStore;
                ZoomOnStore(viewModel.LocatedStore);
            }
            else
            {
                viewModel.SelectedStore = selectedStore;
                ZoomOnStore(viewModel.SelectedStore);
            }
            LocateButton.Focus(FocusState.Programmatic);
        }

        private void ZoomOnStore(Store store)
        {
            double X = store.PositionX * MapScrollViewer.ZoomFactor - Window.Current.Bounds.Width / 2;
            double Y = store.PositionY * MapScrollViewer.ZoomFactor - Window.Current.Bounds.Height / 2;
            
            TimeSpan period = TimeSpan.FromMilliseconds(200);

            Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MapScrollViewer.ChangeView(X, Y, null, false);
                });
            }
            , period);
        }

        private void ZoomOut()
        {
            if (DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor0Width == 0)
                return;
            TimeSpan period = TimeSpan.FromMilliseconds(200);
            float factor = (float)(Window.Current.Bounds.Width / DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor0Width);
            if (factor > 1f)
                factor = 1f;
            double X = (DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor0Width / 2) * factor;
            double Y = (DataStorage.Centres[DataStorage.Centres.IndexOf(viewModel.Centre)].Floor0Height / 2) * factor;
            Windows.System.Threading.ThreadPoolTimer.CreateTimer(async (source) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    MapScrollViewer.ChangeView(X, Y, factor, false);
                });
            }
            , period);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.IsSuggestionListOpen = true;
        }

        private void LocationToPointInStore()
        {
            Point A = new Point(viewModel.Centre.ALongitude, viewModel.Centre.ALatitude);
            Point B = new Point(viewModel.Centre.BLongitude, viewModel.Centre.BLatitude);
            Point C = new Point(viewModel.Centre.CLongitude, viewModel.Centre.CLatitude);
            Point P = new Point(longitude, latitude);

            double width = Math.Sqrt((B.X - A.X) * (B.X - A.X) + (B.Y - A.Y) * (B.Y - A.Y));
            double height = Math.Sqrt((C.X - A.X) * (C.X - A.X) + (C.Y - A.Y) * (C.Y - A.Y));
            double a, b, c, p, s, S, v_p;

            a = Math.Sqrt((P.X - C.X) * (P.X - C.X) + (P.Y - C.Y) * (P.Y - C.Y));
            c = Math.Sqrt((P.X - A.X) * (P.X - A.X) + (P.Y - A.Y) * (P.Y - A.Y));
            p = height; //Math.Sqrt((A.X - C.X) * (A.X - C.X) + (A.Y - C.Y) * (A.Y - C.Y));
            s = (a + c + p) / 2;
            S = Math.Sqrt(s * (s - a) * (s - c) * (s - p));
            v_p = 2 * S / p;
                    
            double xpomer = v_p / width;

            a = Math.Sqrt((P.X - B.X) * (P.X - B.X) + (P.Y - B.Y) * (P.Y - B.Y));
            b = c; //Math.Sqrt((P.X - A.X) * (P.X - A.X) + (P.Y - A.Y) * (P.Y - A.Y));
            p = width; //Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
            s = (a + b + p) / 2;
            S = Math.Sqrt(s * (s - a) * (s - b) * (s - p));
            v_p = 2 * S / p;
            double ypomer = v_p / height;

            int x = (int)(viewModel.Centre.Floor0Width * xpomer);
            int y = (int)(viewModel.Centre.Floor0Height * ypomer);

            viewModel.YourPosition = x + "," + y;
            viewModel.YourPositionVisibility = true;
        }

        private async void LocateButton_Click(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    if (geoLocator == null)
                    {
                        geoLocator = new Geolocator { DesiredAccuracyInMeters = 5 };
                        geoLocator.StatusChanged += OnStatusChanged;
                    }
                    if (!geoLocatorPositionChangedAssigned)
                    {
                        geoLocator.PositionChanged += OnPositionChanged;
                        geoLocatorPositionChangedAssigned = true;
                    }
                    break;
                case GeolocationAccessStatus.Denied:
                    PopUpLocationDenied();
                    break;
                case GeolocationAccessStatus.Unspecified:
                    PopUpUnspecifiedError();
                    break;
            }
            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                
                //TODO? keď užívateľ zmení nastavenia, dá sa to nájsť tu: https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/get-location
            }
            else
            {
                
                //TODO? doplniť text o tom ako povoliť lokalizáciu pre apku (tiez v hornom linku snad je)
            }   
        }

        private async Task UpdateLocationData(Geolocator geoLocator)
        {
            if (geoLocator != null)
            {
                Geoposition pos = await geoLocator.GetGeopositionAsync();
                latitude = pos.Coordinate.Point.Position.Latitude;
                longitude = pos.Coordinate.Point.Position.Longitude;
                // fake pos 49.187558, 16.614713 stred fontany
                latitude = 49.187558;
                longitude = 16.614713;
                // fake pos 49.187469, 16.614156 zapadny vchod (na mapke dole v strede)
                //latitude = 49.187469;
                //longitude = 16.614156;
            }
        }

        private bool CheckIfInCentre()
        {
            return (latitude < viewModel.Centre.MaxLatitude) && (latitude > viewModel.Centre.MinLatitude)
                    && (longitude < viewModel.Centre.MaxLongitude) && (longitude > viewModel.Centre.MinLongitude);
        }

        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await UpdateLocationData(sender);
                if (CheckIfInCentre())
                {
                    LocationToPointInStore();
                }
                else
                {
                    geoLocator.PositionChanged -= OnPositionChanged;
                    geoLocatorPositionChangedAssigned = false;
                    PopUpLocationOutOfCentre();
                }
            });
        }

        async private void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                switch (e.Status)
                {
                    case PositionStatus.Ready:
                        // Location platform is providing valid data.
                        ScenarioOutput_Status.Text = "Ready";
                        break;

                    case PositionStatus.Initializing:
                        // Location platform is attempting to acquire a fix.
                        ScenarioOutput_Status.Text = "Initializing";
                        break;
                        
                    case PositionStatus.NoData:
                        // Location platform could not obtain location data.
                        ScenarioOutput_Status.Text = "No data";
                        break;

                    case PositionStatus.Disabled:
                        // The permission to access location data is denied by the user or other policies.
                        ScenarioOutput_Status.Text = "Disabled";

                        // Show message to the user to go to location settings.
                        //LocationDisabledMessage.Visibility = Visibility.Visible;
                        break;

                    case PositionStatus.NotInitialized:
                        // The location platform is not initialized. This indicates that the application
                        // has not made a request for location data.
                        ScenarioOutput_Status.Text = "Not initialized";
                        break;

                    case PositionStatus.NotAvailable:
                        // The location platform is not available on this version of the OS.
                        ScenarioOutput_Status.Text = "Not available";
                        break;

                    default:
                        ScenarioOutput_Status.Text = "Unknown";
                        break;
                }
            });
        }


        private void PopUpLocationOutOfCentre()
        {
            PopUpLocationError(resourceLoader.GetString("LocationOutOfCentre"));
        }

        private void PopUpLocationDenied()
        {
            PopUpLocationError(resourceLoader.GetString("LocationDenied"));
        }

        private void PopUpUnspecifiedError()
        {
            PopUpLocationError(resourceLoader.GetString("LocationError"));
        }

        private async void PopUpLocationError(string title)
        {
            ContentDialog locationPromptDialog = new ContentDialog
            {
                Title = title,
                Content = "",
                PrimaryButtonText = resourceLoader.GetString("OK"),
                SecondaryButtonText = resourceLoader.GetString("LocationInputStore")

            };

            ContentDialogResult result = await locationPromptDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                LocateButton.Focus(FocusState.Programmatic);
            }
            if (result == ContentDialogResult.Secondary)
            {
                selectingManualPosition = true;
                viewModel.NameFilter = "";
                SearchBox.Focus(FocusState.Programmatic);
            }
        }

        private void LocateButton_LayoutUpdated(object sender, object e)
        {
            if (pageLoaded)
            {
                LocateButton.Focus(FocusState.Programmatic);
                pageLoaded = false;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            selectingManualPosition = false;
        }
    }
}
