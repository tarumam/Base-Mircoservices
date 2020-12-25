using System;
using System.Text.Json.Serialization;
using FluentValidation;

namespace BaseProject.ShoppingCart.API.Model
{
    public class ShoppingCartItem
    {
        public ShoppingCartItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public Guid ShoppingCartId { get; set; }

        [JsonIgnore]
        public ShoppingCartClient ShoppingCartClient { get; set; }

        internal void AssociateShoppingCart(Guid shoppingCartId)
        {
            ShoppingCartId = shoppingCartId;
        }

        internal decimal CalculateValue()
        {
            return Amount * Value;
        }

        internal void AddUnities(int unities)
        {
            Amount += unities;
        }

        internal void UpdateUnities(int unities)
        {
            Amount = unities;
        }
        internal bool IsValid()
        {
            return new ShoppingCartItemValidation().Validate(this).IsValid;
        }

        public class ShoppingCartItemValidation : AbstractValidator<ShoppingCartItem>
        {
            public ShoppingCartItemValidation()
            {
                RuleFor(c => c.ProductId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("O Nome do produto não foi informado");

                RuleFor(c => c.Amount)
                    .GreaterThan(0)
                    .WithMessage(item => $"A quantidade mínima de um item {item.Name} é 1");

                RuleFor(c => c.Value)
                    .GreaterThan(0)
                    .WithMessage(item=> $"O valor do item {item.Name} precisa ser maior que 0");
            }
        }
    }
}
