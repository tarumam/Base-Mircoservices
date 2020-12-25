using System;
using BaseProject.Core.Messages;

namespace BaseProject.Catalog.API.Application.Events
{
    public class ProductAddedEvent : Event
    {
        public ProductAddedEvent(Guid id, string barcode, string name, string description, bool active, DateTime createdAt, string image)
        {
            Id = id;
            Barcode = barcode;
            Name = name;
            Description = description;
            Active = active;
            CreatedAt = createdAt;
            Image = image;
        }

        public Guid Id { get; private set; }
        public string Barcode { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Image { get; private set; }
    }
}
