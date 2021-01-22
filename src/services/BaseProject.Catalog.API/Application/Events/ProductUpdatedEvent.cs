using System;
using BaseProject.Core.Messages;

namespace BaseProject.Catalog.API.Application.Events
{
    public class ProductUpdatedEvent : Event
    {
        public ProductUpdatedEvent(Guid id, string barcode, string name, string description, bool active, string image)
        {
            Id = id;
            Barcode = barcode;
            Name = name;
            Description = description;
            Active = active;
            Image = image;
        }

        public Guid Id { get; private set; }
        public string Barcode { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public string Image { get; private set; }
    }
}
