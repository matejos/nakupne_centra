using nakupne_centra.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
