using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakupne_centra.DataModel
{
    class CentreStoreSearch
    {
        public CentreStoreSearch(Centre centre, string query = "", Store store = null, bool focusSearchBar = false)
        {
            this.Centre = centre;
            this.Query = query;
            this.Store = store;
            this.FocusSearchBar = focusSearchBar;
        }

        public Centre Centre { get; private set; }
        public string Query { get; private set; }
        public Store Store { get; private set; }
        public bool FocusSearchBar { get; private set; }
    }
}
