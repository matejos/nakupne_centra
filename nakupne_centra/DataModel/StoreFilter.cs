using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakupne_centra.DataModel
{
    static public class StoreFilter
    {
        public static bool Match(string name, string filter)
        {
            var words = name.ToLower().Split(' ');
            foreach(var word in words)
            {
                if (word.StartsWith(filter.ToLower()))
                    return true;
            }
            return false;
        }
    }
}
