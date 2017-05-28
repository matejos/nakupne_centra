using nakupne_centra.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace nakupne_centra.ViewModel
{
    public class StoresListViewModel : INotifyPropertyChanged
    {
        public StoresListViewModel(Centre centre)
        {
            Centre = centre;
            Name = Centre.Name;
            Stores = centre.Stores;
            FilteredStores = Stores;
            Map = Centre.Floor0;
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

        private Store _selectedStore;

        public Store SelectedStore
        {
            get { return _selectedStore; }
            set { _selectedStore = value; NotifyPropertyChanged("SelectedStore"); RefreshSelectedStore(); }
        }

        private ImageSource _map;

        public ImageSource Map
        {
            get { return _map; }
            set { _map = value; NotifyPropertyChanged("Map"); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        private string _storeName;

        public string StoreName
        {
            get { return _storeName; }
            set { _storeName = value; NotifyPropertyChanged("StoreName"); }
        }

        private string _storeDesc;

        public string StoreDesc
        {
            get { return _storeDesc; }
            set { _storeDesc = value; NotifyPropertyChanged("StoreDesc"); }
        }

        private Hours _hours;

        public Hours Hours
        {
            get { return _hours; }
            set { _hours = value; NotifyPropertyChanged("Hours"); }
        }

        public void RefreshFilteredData()
        {
            var fs = from fobjs in Stores
                     where fobjs.Name.ToLower().Contains(NameFilter.ToLower())
                     select fobjs;
            
            ObservableCollection<Store> newFilteredStores = new ObservableCollection<Store>(fs);

            if (FilteredStores.Except(newFilteredStores).Count() == 0 && newFilteredStores.Except(FilteredStores).Count() == 0)
                return;

            FilteredStores = newFilteredStores;
        }

        private void RefreshSelectedStore()
        {
            StoreName = SelectedStore.Name;
            StoreDesc = SelectedStore.Description;
            Hours = SelectedStore.StoreHours;
            Map = Centre.LogoRect;
            string storeFloor = SelectedStore.Floor;
            if (storeFloor.Equals("0"))
            {
                Map = Centre.Floor0;
            }
            else
            {
                Map = Centre.Floor1;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
