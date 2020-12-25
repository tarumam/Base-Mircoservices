using System;
using BaseProject.Core.Messages;
using FluentValidation;

namespace BaseProject.Catalog.API.Application.Commands
{
    public class AddProductCommand : Command
    {
        public AddProductCommand(string barcode, string name, string description, bool active, string image)
        {
            Id = Guid.NewGuid();
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
        public override bool IsValid()
        {
            ValidationResult = new AddProductValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddProductValidation : AbstractValidator<AddProductCommand>
        {
            public AddProductValidation()
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do produto inválido");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("O nome do produto não foi informado");
            }
        }
    }
}
