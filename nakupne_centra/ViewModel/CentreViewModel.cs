using nakupne_centra.DataModel;
using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace nakupne_centra.ViewModel
{
    public class CentreViewModel : INotifyPropertyChanged
    {
        public CentreViewModel(Centre centre)
        {
            Centre = centre;
            Name = Centre.Name;
            Hours = Centre.Hours;
            LogoRect = Centre.LogoRect;
            LogoSquare = Centre.LogoSquare;
            LogoColor = Centre.LogoColor;
            Map = Centre.Floor0;
        }

        private Centre _centre;

        public Centre Centre
        {
            get { return _centre; }
            set { _centre = value; NotifyPropertyChanged("Centre"); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        private string _logoColor;

        public string LogoColor
        {
            get { return _logoColor; }
            set { _logoColor = value; NotifyPropertyChanged("LogoColor"); }
        }

        private Hours _hours;

        public Hours Hours
        {
            get { return _hours; }
            set { _hours = value; NotifyPropertyChanged("Hours"); }
        }

        private ImageSource _logoSquare;

        public ImageSource LogoSquare
        {
            get { return _logoSquare; }
            set { _logoSquare = value; NotifyPropertyChanged("LogoSquare"); }
        }

        private ImageSource _logoRect;

        public ImageSource LogoRect
        {
            get { return _logoRect; }
            set { _logoRect = value; NotifyPropertyChanged("LogoRect"); }
        }

        private ImageSource _map;

        public ImageSource Map
        {
            get { return _map; }
            set { _map = value; NotifyPropertyChanged("Map"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
