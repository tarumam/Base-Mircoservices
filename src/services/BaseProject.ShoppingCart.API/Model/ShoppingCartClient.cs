using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace BaseProject.ShoppingCart.API.Model
{
    public class ShoppingCartClient
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal TotalValue { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public ValidationResult ValidationResult { get; set; }
        public ShoppingCartClient(Guid clientId)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
        }
        public ShoppingCartClient() { }
        internal void CalculateShoppingCartValue()
        {
            TotalValue = Items.Sum(p => p.CalculateValue());
        }
        internal ShoppingCartItem GetProductById(Guid productId)
        {
            return Items.FirstOrDefault(p => p.ProductId == productId);
        }
        internal void AddItem(ShoppingCartItem item)
        {
            item.AssociateShoppingCart(Id);

            if (IsItemExistentInShoppingCart(item))
            {
                var existentItem = GetProductById(item.ProductId);
                existentItem.AddUnities(item.Amount);

                item = existentItem;
                Items.Remove(existentItem);
            }
            Items.Add(item);
            CalculateShoppingCartValue();
        }
        internal void UpdateItem(ShoppingCartItem item)
        {
            item.AssociateShoppingCart(Id);

            var existentItem = GetProductById(item.ProductId);

            Items.Remove(existentItem);
            Items.Add(item);

            CalculateShoppingCartValue();
        }
        internal void RemoveItem(ShoppingCartItem item)
        {
            Items.Remove(GetProductById(item.ProductId));

            CalculateShoppingCartValue();
        }
        internal void UpdateUnities(ShoppingCartItem item, int unities)
        {
            item.UpdateUnities(unities);
            UpdateItem(item);
        }
        internal bool IsItemExistentInShoppingCart(ShoppingCartItem item)
        {
            return Items.Any(predicate => predicate.ProductId == item.ProductId);
        }
        internal bool IsValid()
        {
            var errors = Items.SelectMany(i => new ShoppingCartItem.ShoppingCartItemValidation().Validate(i).Errors).ToList();
            errors.AddRange(new ShoppingCartClientValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(errors);
            return ValidationResult.IsValid;
        }
        public class ShoppingCartClientValidation : AbstractValidator<ShoppingCartClient>
        {
            public ShoppingCartClientValidation()
            {
                RuleFor(c => c.ClientId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(c => c.Items.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(c => c.TotalValue)
                    .GreaterThan(0)
                    .WithMessage(item => $"O valor total do carrinho precisa ser maior que 0");
            }
        }
    }
}
