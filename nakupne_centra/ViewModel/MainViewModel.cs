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
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Centres = new ObservableCollection<Centre>();
            LoadData();
        }

        private ObservableCollection<Centre> _centres;

        public ObservableCollection<Centre> Centres
        {
            get { return _centres; }
            set { _centres = value; NotifyPropertyChanged("Centres"); }
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

        private void RefreshFilteredData()
        {
            var fr = from fobjs in Stores
                     where fobjs.Name.ToLower().Contains(NameFilter.ToLower())
                     select fobjs;

            //  This will limit the amount of view refreshes
            if (FilteredStores == null || FilteredStores.Count == fr.Count())
                return;

            FilteredStores = new ObservableCollection<Store>(fr);
        }

        private async void LoadData()
        {
            if (this._centres.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///DataModel/CentresData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Centres"].GetArray();

            foreach (JsonValue centreJson in jsonArray)
            {
                JsonObject centreObject = centreJson.GetObject();
                Hours centreHours = new Hours(centreObject["Hours"].GetObject()["Monday"].GetString(), centreObject["Hours"].GetObject()["Tuesday"].GetString(),
                    centreObject["Hours"].GetObject()["Wednesday"].GetString(), centreObject["Hours"].GetObject()["Thursday"].GetString(),
                    centreObject["Hours"].GetObject()["Friday"].GetString(), centreObject["Hours"].GetObject()["Saturday"].GetString(),
                    centreObject["Hours"].GetObject()["Sunday"].GetString());
                ObservableCollection<Store> stores = new ObservableCollection<Store>();
                foreach (JsonValue storeJson in centreObject["Stores"].GetArray())
                {
                    JsonObject storeObject = storeJson.GetObject();
                    Hours storeHours = new Hours(storeObject["Hours"].GetObject()["Monday"].GetString(), storeObject["Hours"].GetObject()["Tuesday"].GetString(),
                    storeObject["Hours"].GetObject()["Wednesday"].GetString(), storeObject["Hours"].GetObject()["Thursday"].GetString(),
                    storeObject["Hours"].GetObject()["Friday"].GetString(), storeObject["Hours"].GetObject()["Saturday"].GetString(),
                    storeObject["Hours"].GetObject()["Sunday"].GetString());
                    stores.Add(new Store(storeObject["Name"].GetString(),
                        storeObject["Description"].GetString(),
                        storeObject["Category"].GetString(),
                        storeObject["Floor"].GetString(),
                        storeHours
                        ));
                }
                string dataFolder = centreObject["DataFolder"].GetString();
                Centres.Add(new Centre(centreObject["Name"].GetString(),
                                                       centreObject["Address"].GetString(),
                                                       new BitmapImage(new Uri(dataFolder + "logoSquare.png")),
                                                       new BitmapImage(new Uri(dataFolder + "logoRect.png")),
                                                       new BitmapImage(new Uri(dataFolder + "floor0.png")),
                                                       new BitmapImage(new Uri(dataFolder + "floor1.png")),
                                                       centreHours,
                                                       stores));
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
