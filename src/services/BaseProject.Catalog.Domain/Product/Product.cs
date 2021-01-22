
using System.Collections.Generic;
using BaseProject.Core.DomainObjects;

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
        public bool SyncWithWeb { get; private set; } = false;
        public virtual List<Price> Prices { get; private set; } = new List<Price>();

        public void SetName(string name)
        {
            if (!string.IsNullOrEmpty(name) && name != this.Name)
                Name = name;
        }

        public void SetDescription(string description)
        {
            if (!string.IsNullOrEmpty(description) && description != this.Description)
                Description = description;
        }

        public void setActive(bool active)
        {
            if (active != this.Active)
                Active = active;
        }

        public void setImage(string image)
        {
            if (!string.IsNullOrEmpty(image) && image != this.Image)
                Image = image;
        }

        public void SetSyncWithWeb(bool sync)
        {
            SyncWithWeb = sync;
        }
    }
}
