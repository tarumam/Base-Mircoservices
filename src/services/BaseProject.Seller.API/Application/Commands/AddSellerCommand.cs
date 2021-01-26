using System;
using BaseProject.Core.Messages;
using FluentValidation;

namespace BaseProject.Seller.API.Application.Commands
{
    public class AddSellerCommand : Command
    {
        public AddSellerCommand(string name, string address, string neighbohood, string number, string reference, string workingTime, string details, string image, bool hasParking)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            Neighbohood = neighbohood;
            Number = number;
            Reference = reference;
            WorkingTime = workingTime;
            Details = details;
            Image = image;
            HasParking = hasParking;
        }
        public Guid Id { get; private set; }
        public string DocRef { get; private set; }
        public string DocType { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Neighbohood { get; private set; }
        public string Number { get; private set; }
        public string Reference { get; private set; }
        public string WorkingTime { get; private set; }
        public string Details { get; private set; }
        public string Image { get; private set; }
        public bool HasParking { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new AddSellerCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public class AddSellerCommandValidation : AbstractValidator<AddSellerCommand>
        {
            public AddSellerCommandValidation()
            {
                RuleFor(c => c.Id)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Id do vendedor inválido");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("O nome do cliente não foi informado");

            }
        }
    }
}
