using System;
using BaseProject.Core.DomainObjects;

namespace BaseProject.Catalog.Domain
{
    public class Price : Entity
    {
        public Price() { }
        public Price(Guid productId, Guid sellerId, decimal value, bool active)
        {
            ProductId = productId;
            SellerId = sellerId;
            Value = value;
            Active = active;
        }

        public Guid ProductId { get; private set; }
        public Guid SellerId { get; private set; }
        public decimal Value { get; private set; }
        public bool Active { get; private set; }
        public virtual Product Product { get; private set; }
        public void SetActive(bool active)
        {
            Active = active;
        }
    }
}
