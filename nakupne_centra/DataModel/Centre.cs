using nakupne_centra.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace nakupne_centra.DataModel
{
    public class Centre
    {
        public Centre(string name, string address, ImageSource logoSquare, ImageSource logoRect, ImageSource floor0, ImageSource floor1, Hours hours)
        {
            this.Name = name;
            this.Address = address;
            this.LogoSquare = logoSquare;
            this.LogoRect = logoRect;
            this.Floor0 = floor0;
            this.Floor1 = floor1;
            this.Hours = hours;
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
    }
}
