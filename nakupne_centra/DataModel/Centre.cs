using nakupne_centra.ViewModel;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace nakupne_centra.DataModel
{
    public class Centre
    {
        public Centre(string name, string address, int MinFloor, int MaxFloor, 
            ImageSource logoSquare, ImageSource logoRect, ImageSource floor0, ImageSource floor1, 
            Hours hours, string logoColor, string wayToCentre,
            double minLat, double maxLat, double minLong, double maxLong)
        {
            this.Name = name;
            this.Address = address;
            this.MinFloor = MinFloor;
            this.MaxFloor = MaxFloor;
            this.LogoSquare = logoSquare;
            this.LogoRect = logoRect;
            this.Floor0 = floor0;
            this.Floor1 = floor1;
            this.Hours = hours;
            this.LogoColor = logoColor;
            this.WayToCentre = wayToCentre;
            this.MinLatitude = minLat;
            this.MaxLatitude = maxLat;
            this.MinLongitude = minLong;
            this.MaxLongitude = maxLong;

            this.Floor0Height = 0;
            this.Floor1Height = 0;
            this.Floor0Width = 0;
        }

        public string Name { get; private set; }
        public string Address { get; private set; }
        public ImageSource LogoSquare { get; private set; }
        public ImageSource LogoRect { get; private set; }
        public ImageSource Floor0 { get; private set; }
        public ImageSource Floor1 { get; private set; }
        public Hours Hours { get; private set; }
        public ObservableCollection<Store> Stores { get; set; }
        public StoresListViewModel viewModel { get; set; }
        public string LogoColor { get; set; }
        public double Floor0Height { get; set; }
        public double Floor0Width { get; set; }
        public double Floor1Height { get; set; }
        public int MinFloor { get; set; }
        public int MaxFloor { get; set; }
        public string WayToCentre { get; set; }
        public double MinLatitude { get; set; }
        public double MaxLatitude { get; set; }
        public double MinLongitude { get; set; }
        public double MaxLongitude { get; set; }
    }
}
