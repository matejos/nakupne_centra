using Blend.SampleData.CentresSampleDataSource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace nakupne_centra.ViewModel
{
    public class StoresListViewModel : INotifyPropertyChanged
    {
        public StoresListViewModel(CentresItem centre)
        {
            Centre = centre;
            Stores = centre.Stores;
            FilteredStores = Stores;
            Map = Centre.LogoRect;
        }

        private CentresItem _centre;

        public CentresItem Centre
        {
            get { return _centre; }
            set { _centre = value; NotifyPropertyChanged("Centre"); }
        }

        private ObservableCollection<StoresItem> _stores;

        public ObservableCollection<StoresItem> Stores
        {
            get { return _stores; }
            set { _stores = value; NotifyPropertyChanged("Stores"); }
        }

        private ObservableCollection<StoresItem> _filteredStores;

        public ObservableCollection<StoresItem> FilteredStores
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

        private StoresItem _selectedStore;

        public StoresItem SelectedStore
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

        private void RefreshFilteredData()
        {
            var fr = from fobjs in Stores
                     where fobjs.Name.ToLower().Contains(NameFilter.ToLower())
                     select fobjs;

            //  This will limit the amount of view refreshes
            if (FilteredStores == null || FilteredStores.Count == fr.Count())
                return;

            FilteredStores = new ObservableCollection<StoresItem>(fr);
        }

        private void RefreshSelectedStore()
        {
            StoreName = SelectedStore.Name;
            StoreDesc = SelectedStore.Desc;
            Map = Centre.LogoRect;
            string storeFloor = SelectedStore.Category;
            if (storeFloor == "0")
            {
                Map = Centre.LogoRect;
            }
            else
            {
                Map = Centre.LogoSquare;
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
