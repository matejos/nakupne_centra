﻿using System;
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
        private int horizontalPosition;
        private int verticalPosition;
        //private Geolocator geoLocator;
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
            viewModel.YourPositionVisibility = true;
            double x = 0, y = 0;
            switch (viewModel.Name)
            {
                case "Galerie Vaňkovka":
                    // y = (1081 - (49.187155 * 159 / 49.188827)) / (16.614252 - (49.187155 * 16.613679 / 49.188827));
                    // x = (159 - (16.613679 * y)) / 49.188827;
                    y = (1081 - (765 * 159 / 2427)) / (724 - (765 * 179 / 2427));
                    x = (159 - (179 * y)) / 2427;
                    //Debug.WriteLine(755 * x + 652 * y);
                    //horizontalPosition = (latitude-viewModel.Centre.MinLatitude)*x + (longitude - viewModel.Centre.MinLongitude) * y; 
                    verticalPosition = 0;
                    break;
                case "Futurum":
                    Debug.WriteLine("ešči neni");
                    break;
            }
            viewModel.YourPosition = x + "," + y;
        }

        private async void LocateButton_Click(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                Geolocator geoLocator = new Geolocator();

                await UpdateLocationData(geoLocator);
                if (CheckIfInCentre())
                {
                    LocationToPointInStore();
                    if (!geoLocatorPositionChangedAssigned)
                    {
                        geoLocator.PositionChanged += OnPositionChanged;
                        geoLocatorPositionChangedAssigned = true;
                    }
                }
                else
                {
                    PopUpLocationOutOfCentre();
                }
                //TODO? keď užívateľ zmení nastavenia, dá sa to nájsť tu: https://docs.microsoft.com/en-us/windows/uwp/maps-and-location/get-location
            }
            else
            {
                PopUpLocationDenied();
                //TODO? doplniť text o tom ako povoliť lokalizáciu pre apku (tiez v hornom linku snad je)
            }   
        }

        private async Task UpdateLocationData(Geolocator geoLocator)
        {
            if (geoLocator != null)
            {
                geoLocator.DesiredAccuracy = PositionAccuracy.High;

                Geoposition pos = await geoLocator.GetGeopositionAsync();
                latitude = pos.Coordinate.Point.Position.Latitude;
                longitude = pos.Coordinate.Point.Position.Longitude;
                // fake pos
                latitude = 49.187637;
                longitude = 16.615180;
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

                }
            });
        }

        private void PopUpLocationOutOfCentre()
        {
            PopUp.Visibility = Visibility.Visible;
            PopUpText.Text = resourceLoader.GetString("LocationOutOfCentre");
            PopUpButton1.Content = resourceLoader.GetString("OK");
            PopUpButton2.Content = resourceLoader.GetString("LocationInputStore");
        }

        private void PopUpLocationDenied()
        {
            PopUp.Visibility = Visibility.Visible;
            PopUpText.Text = resourceLoader.GetString("LocationDenied");
            PopUpButton1.Content = resourceLoader.GetString("OK");
            PopUpButton2.Content = resourceLoader.GetString("LocationInputStore");
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

        private void PopUpButton1_Click(object sender, RoutedEventArgs e)
        {
            if (true)   // ked to ma byt OK button
            {
                LocateButton.Focus(FocusState.Programmatic);
                PopUp.Visibility = Visibility.Collapsed;
            }
        }

        private void PopUpButton2_Click(object sender, RoutedEventArgs e)
        {
            if (true)   // ked to ma byt find store button
            {
                PopUp.Visibility = Visibility.Collapsed;
                selectingManualPosition = true;
                viewModel.NameFilter = "";
            }
        }
    }
}
