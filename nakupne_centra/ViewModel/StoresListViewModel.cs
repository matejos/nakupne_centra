﻿using nakupne_centra.DataModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Media;

namespace nakupne_centra.ViewModel
{
    public class StoresListViewModel : INotifyPropertyChanged
    {
        public StoresListViewModel(Centre centre)
        {
            Centre = centre;
            Name = Centre.Name;
            Stores = new ObservableCollection<Store>(from i in centre.Stores orderby i.Name select i);
            FilteredStores = Stores;
            Map = Centre.Floor0;
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

        private ObservableCollection<Store> _filteredStores;

        public ObservableCollection<Store> FilteredStores
        {
            get { return _filteredStores; }
            set { _filteredStores = value; NotifyPropertyChanged("FilteredStores"); }
        }

        private SortedDictionary<string, ObservableCollection<Store>> _filteredCategories;

        public SortedDictionary<string, ObservableCollection<Store>> FilteredCategories
        {
            get { return _filteredCategories; }
            set { _filteredCategories = value; NotifyPropertyChanged("FilteredCategories"); }
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

        private double _mapHeight;

        public double MapHeight
        {
            get { return _mapHeight; }
            set { _mapHeight = value; NotifyPropertyChanged("MapHeight"); }
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

        private string _storeCategory;

        public string StoreCategory
        {
            get { return _storeCategory; }
            set { _storeCategory = value; NotifyPropertyChanged("StoreCategory"); }
        }

        private string _storePosition;

        public string StorePosition
        {
            get { return _storePosition; }
            set { _storePosition = value; NotifyPropertyChanged("StorePosition"); }
        }

        private Hours _hours;

        public Hours Hours
        {
            get { return _hours; }
            set { _hours = value; NotifyPropertyChanged("Hours"); }
        }

        public int RefreshFilteredData()
        {
            var filter = NameFilter.ToLower();
            ObservableCollection<Store> newFilteredStores;
            SortedDictionary<string, ObservableCollection<Store>> newFilteredCategories = new SortedDictionary<string, ObservableCollection<Store>>();

            newFilteredStores = new ObservableCollection<Store>();
            foreach (Store store in Stores)
            {
                if (StoreFilter.Match(store.Name, filter))
                {
                    newFilteredStores.Add(store);
                    if (!newFilteredCategories.ContainsKey(store.Category))
                    {
                        newFilteredCategories.Add(store.Category, new ObservableCollection<Store>());
                    }
                    newFilteredCategories[store.Category].Add(store);
                }
            }
            
            FilteredCategories = newFilteredCategories;
            

            if (FilteredStores.Except(newFilteredStores).Count() == 0 && newFilteredStores.Except(FilteredStores).Count() == 0)
                return FilteredStores.Count;

            FilteredStores = newFilteredStores;
            return FilteredStores.Count;
        }


        private void RefreshSelectedStore()
        {
            if (SelectedStore != null)
            {
                StoreName = SelectedStore.Name;
                StoreDesc = SelectedStore.Description;
                StoreCategory = ResourceLoader.GetForCurrentView().GetString("Category" + SelectedStore.Category);
                if (StoreCategory == "")
                    StoreCategory = SelectedStore.Category;
                StorePosition = SelectedStore.PositionX + "," + SelectedStore.PositionY;
                Hours = SelectedStore.StoreHours;
                string storeFloor = SelectedStore.Floor;
                if (storeFloor.Equals("0"))
                {
                    Map = Centre.Floor0;
                    MapHeight = DataStorage.Centres[DataStorage.Centres.IndexOf(Centre)].Floor0Height;
                }
                else if (storeFloor.Equals("1"))
                {
                    Map = Centre.Floor1;
                    MapHeight = DataStorage.Centres[DataStorage.Centres.IndexOf(Centre)].Floor1Height;
                }
            }
            else
            {
                Map = Centre.Floor0;
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
