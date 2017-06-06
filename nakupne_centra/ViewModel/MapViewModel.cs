using nakupne_centra.DataModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace nakupne_centra.ViewModel
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public MapViewModel(Centre centre, Store store)
        {
            Centre = centre;
            Map = Centre.Floor0;
            Stores = new ObservableCollection<Store>(from i in centre.Stores orderby i.Name select i);
            Name = Centre.Name;
            SelectedStore = store;

            MinFloor = Centre.MinFloor;
            MaxFloor = Centre.MaxFloor;
            Floors = Centre.MaxFloor - Centre.MinFloor + 1;

            
            FilteredStores = Stores;
            NameFilter = "";
        }

        private Centre _centre;

        public Centre Centre
        {
            get { return _centre; }
            set { _centre = value; NotifyPropertyChanged("Centre"); }
        }

        private ObservableCollection<Store> _stores;

        public ObservableCollection<Store> Stores
        {
            get { return _stores; }
            set { _stores = value; NotifyPropertyChanged("Stores"); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        private string _storePosition;

        public string StorePosition
        {
            get { return _storePosition; }
            set { _storePosition = value; NotifyPropertyChanged("StorePosition"); }
        }

        private bool _storePositionVisibility;

        public bool StorePositionVisibility
        {
            get { return _storePositionVisibility; }
            set { _storePositionVisibility = value; NotifyPropertyChanged("StorePositionVisibility"); }
        }

        private string _yourPosition;

        public string YourPosition
        {
            get { return _yourPosition; }
            set { _yourPosition = value; NotifyPropertyChanged("YourPosition"); }
        }

        private bool __yourPositionVisibility;

        public bool YourPositionVisibility
        {
            get { return __yourPositionVisibility; }
            set { __yourPositionVisibility = value; NotifyPropertyChanged("YourPositionVisibility"); }
        }

        private bool _locationOnline;

        public bool LocationOnline
        {
            get { return _locationOnline; }
            set { _locationOnline = value; NotifyPropertyChanged("LocationOnline"); }
        }

        private Store _selectedStore;

        public Store SelectedStore
        {
            get { return _selectedStore; }
            set { _selectedStore = value; NotifyPropertyChanged("SelectedStore"); RefreshSelectedStore(); }
        }

        private Store _locatedStore;

        public Store LocatedStore
        {
            get { return _locatedStore; }
            set { _locatedStore = value; NotifyPropertyChanged("LocatedStore"); RefreshLocatedStore(); }
        }

        private int _floors;

        public int Floors
        {
            get { return _floors; }
            set { _floors = value; NotifyPropertyChanged("Floors"); }
        }

        private int _minFloor;

        public int MinFloor
        {
            get { return _minFloor; }
            set { _minFloor = value; NotifyPropertyChanged("MinFloor"); }
        }


        private int _maxFloor;

        public int MaxFloor
        {
            get { return _maxFloor; }
            set { _maxFloor = value; NotifyPropertyChanged("MaxFloor"); }
        }


        private ImageSource _map;

        public ImageSource Map
        {
            get { return _map; }
            set { _map = value; NotifyPropertyChanged("Map"); }
        }

        private ObservableCollection<Store> _filteredStores;

        public ObservableCollection<Store> FilteredStores
        {
            get { return _filteredStores; }
            set { _filteredStores = value; NotifyPropertyChanged("FilteredStores"); }
        }

        private string _nameFilter;

        public string NameFilter
        {
            get { return _nameFilter; }
            set { _nameFilter = value; NotifyPropertyChanged("NameFilter"); RefreshFilteredData(); }
        }

        private int _floorSliderValue;

        public int FloorSliderValue
        {
            get { return _floorSliderValue; }
            set { _floorSliderValue = value; NotifyPropertyChanged("FloorSliderValue"); ChangeFloor(); }
        }

        public void RefreshFilteredData()
        {
            var filter = NameFilter.ToLower();
            ObservableCollection<Store> newFilteredStores = new ObservableCollection<Store>();
            foreach (Store store in Stores)
            {
                if (StoreFilter.Match(store.Name, filter))
                {
                    newFilteredStores.Add(store);
                }
            }

            if (FilteredStores.Except(newFilteredStores).Count() == 0 && newFilteredStores.Except(FilteredStores).Count() == 0)
                return;

            FilteredStores = newFilteredStores;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        void RefreshSelectedStore()
        {
            if (SelectedStore != null)
            {
                switch (SelectedStore.Floor)
                {
                    case "0":
                        FloorSliderValue = 0;
                        break;
                    case "1":
                        FloorSliderValue = 1;
                        break;
                };
                StorePositionVisibility = true;
                StorePosition = SelectedStore.PositionX + "," + SelectedStore.PositionY;
            }
            else
            {
                StorePositionVisibility = false;
            }
        }

        void RefreshLocatedStore()
        {
            if (LocatedStore != null)
            {
                switch (LocatedStore.Floor)
                {
                    case "0":
                        FloorSliderValue = 0;
                        break;
                    case "1":
                        FloorSliderValue = 1;
                        break;
                };
                YourPositionVisibility = true;
                YourPosition = LocatedStore.PositionX + "," + LocatedStore.PositionY;
            }
            else
            {
                YourPositionVisibility = false;
            }
        }

        void ChangeFloor()
        {
            if (FloorSliderValue == 0)
            {
                Map = Centre.Floor0;
            }
            else if (FloorSliderValue == 1)
            {
                Map = Centre.Floor1;
            }
            if (SelectedStore != null)
            {
                if (SelectedStore.Floor.Contains(FloorSliderValue.ToString()))
                {
                        StorePositionVisibility = true;
                }
                else
                {
                    StorePositionVisibility = false;
                }
            }
            if (LocatedStore != null && !LocationOnline)
            {
                if (LocatedStore.Floor.Contains(FloorSliderValue.ToString()))
                {
                    YourPositionVisibility = true;
                }
                else
                {
                    YourPositionVisibility = false;
                }
            }
        }
    }
}
