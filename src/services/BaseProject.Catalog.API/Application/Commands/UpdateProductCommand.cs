using System;
using BaseProject.Core.Messages;
using FluentValidation;

namespace BaseProject.Catalog.API.Application.Commands
{
    public class UpdateProductCommand : Command
    {
        public UpdateProductCommand(Guid id, string barcode, string name, string description, bool active, string image, bool syncWithWeb)
        {
            Id = id;
            Barcode = barcode;
            Name = name;
            Description = description;
            Active = active;
            Image = image;
            SyncWithWeb = syncWithWeb;
        }

        public Guid Id { get; private set; }
        public string Barcode { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public string Image { get; private set; }
        public bool SyncWithWeb { get; private set; }
        public override bool IsValid()
        {
            ValidationResult = new UpdateProductValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class UpdateProductValidation : AbstractValidator<UpdateProductCommand>
        {
            public UpdateProductValidation()
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
