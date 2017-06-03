using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakupne_centra.DataModel
{
    class MapCentreStore
    {
        public MapCentreStore(Centre centre, Store store = null)
        {
            this.Centre = centre;
            this.Store = store;
        }

        public Centre Centre { get; private set; }
        public Store Store { get; private set; }
    }
}
