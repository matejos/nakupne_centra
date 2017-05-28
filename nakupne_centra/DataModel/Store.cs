using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nakupne_centra.DataModel
{
    public class Store
    {
        public Store(string name, string description, string category, string floor, Hours hours)
        {
            this.Name = name;
            this.Description = description;
            this.Category = category;
            this.Floor = floor;
            this.StoreHours = hours;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string Floor { get; private set; }
        public Hours StoreHours { get; private set; }
    }
}
