using System;
using BaseProject.Core.Messages;
using FluentValidation;

namespace BaseProject.Catalog.API.Application.Commands
{
    public class AddPriceCommand : Command
    {
        public AddPriceCommand(Guid? productId, string barcode, Guid sellerId, decimal value)
        {
            Id = Guid.NewGuid();
            Barcode = barcode;
            ProductId = productId;
            SellerId = sellerId;
            Value = value;
            Active = true;
        }

        public Guid Id { get; private set; }
        public Guid? ProductId { get; private set; }
        public string Barcode { get; private set; }
        public Guid SellerId { get; private set; }
        public decimal Value { get; private set; }
        public bool Active { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new AddPriceValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddPriceValidation : AbstractValidator<AddPriceCommand>
        {
            public AddPriceValidation()
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do preço inválido");

                RuleFor(c => c.ProductId)
                    .NotEmpty().When(a => string.IsNullOrEmpty(a.Barcode))
                    .WithMessage("A identificação do produto não foi informada");

                RuleFor(c => c.Barcode)
                    .NotEmpty().When(a => a.ProductId == null || a.ProductId == Guid.Empty)
                    .WithMessage("A identificação do produto não foi informada");

                RuleFor(c => c.SellerId)
                    .NotEmpty()
                    .WithMessage("O id do vendedor não foi informado");

                RuleFor(c => c.Value)
                    .NotEmpty()
                    .WithMessage("O valor do produto não foi informado.");
            }
        }
    }
}
