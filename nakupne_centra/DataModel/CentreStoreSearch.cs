using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakupne_centra.DataModel
{
    class CentreStoreSearch
    {
        public CentreStoreSearch(Centre centre, string query = "")
        {
            this.Centre = centre;
            this.Query = query;
        }

        public Centre Centre { get; private set; }
        public string Query { get; private set; }
    }
}
