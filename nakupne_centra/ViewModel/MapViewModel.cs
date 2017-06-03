﻿using nakupne_centra.DataModel;
using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace nakupne_centra.ViewModel
{
    public class MapViewModel : INotifyPropertyChanged
    {
        public MapViewModel(Centre centre, Store store)
        {
            Centre = centre;
            Name = Centre.Name;
            SelectedStore = store;

            MinFloor = Centre.MinFloor;
            MaxFloor = Centre.MaxFloor;
            Floors = Centre.MaxFloor - Centre.MinFloor + 1;

            Map0 = Centre.Floor0;
            Map1 = Centre.Floor1;

            RefreshSelectedStore();
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

        private string _storePosition;

        public string StorePosition
        {
            get { return _storePosition; }
            set { _storePosition = value; NotifyPropertyChanged("StorePosition"); }
        }

        private Store _selectedStore;

        public Store SelectedStore
        {
            get { return _selectedStore; }
            set { _selectedStore = value; NotifyPropertyChanged("SelectedStore"); RefreshSelectedStore(); }
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


        private ImageSource _map0;

        public ImageSource Map0
        {
            get { return _map0; }
            set { _map0 = value; NotifyPropertyChanged("Map"); }
        }

        private ImageSource _map1;

        public ImageSource Map1
        {
            get { return _map1; }
            set { _map1 = value; NotifyPropertyChanged("Map"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void RefreshSelectedStore()
        {
            //TODO
            //zmeni poschodie a presunie ikonku miesta na poziciu obchodu
            if (SelectedStore != null)
            {
                StorePosition = SelectedStore.PositionX + "," + SelectedStore.PositionY;
            }
            

            /*if (SelectedStore != null)
            {
                StorePosition = SelectedStore.PositionX + "," + SelectedStore.PositionY;
                Hours = SelectedStore.StoreHours;
                string storeFloor = SelectedStore.Floor;
                if (storeFloor.Equals("0"))
                {
                    Map = Centre.Floor0;
                    MapHeight = DataStorage.Centres[DataStorage.Centres.IndexOf(Centre)].Floor0Height;
                }
                else
                {
                    Map = Centre.Floor1;
                    MapHeight = DataStorage.Centres[DataStorage.Centres.IndexOf(Centre)].Floor1Height;
                }
            }
            else
            {
                Map = Centre.Floor0;
            }*/
        }
    }
}