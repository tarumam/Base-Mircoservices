
using System.Collections.Generic;
using BaseProject.Core.DomainObjects;
using System.Linq;

namespace BaseProject.Catalog.Domain
{
    public class Product : Entity, IAggregateRoot
    {
        public Product() { }

        public Product(string barcode, string name, string description, bool active, string image, Price price = null)
        {
            Barcode = barcode;
            Name = name;
            Description = description;
            Active = active;
            Image = image;

            if (price != null)
            {
                Prices.Add(price);
            }
        }

        public string Barcode { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public string Image { get; private set; }
        public List<Price> Prices { get; private set; } = new List<Price>();

    }
}
