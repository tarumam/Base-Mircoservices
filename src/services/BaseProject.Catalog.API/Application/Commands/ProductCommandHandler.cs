using System;
using System.Threading;
using System.Threading.Tasks;
using BaseProject.Catalog.API.Application.Events;
using BaseProject.Catalog.Domain;
using BaseProject.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace BaseProject.Catalog.API.Application.Commands
{
    public class ProductCommandHandler : CommandHandler,
        IRequestHandler<AddProductCommand, ValidationResult>,
        IRequestHandler<AddPriceCommand, ValidationResult>
    {
        private readonly IProductRepository _productRep;

        public ProductCommandHandler(IProductRepository productRep)
        {
            _productRep = productRep;
        }

        public async Task<ValidationResult> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;


            if (!string.IsNullOrEmpty(request.Barcode))
            {
                var existentProduct = await _productRep.GetByBarcode(request.Barcode);
                if (existentProduct != null)
                {
                    AdicionarErro("O código de barras informado já está em uso.");
                    return ValidationResult;
                }
            }
            else
            {
                var existentProduct = await _productRep.GetByName(request.Name);
                if (existentProduct != null)
                {
                    AdicionarErro("O nome informado já existe.");
                    return ValidationResult;
                }
            }

            var product = new Product(request.Barcode, request.Name, request.Description, request.Active, request.Image);

            _productRep.Add(product);

            product.AddEvent(new ProductAddedEvent(product.Id, product.Barcode, product.Name, product.Description, product.Active, product.CreatedAt, product.Image));

            return await PersistirDados(_productRep.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(AddPriceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            var product = await _productRep.GetProductWithPrices(request.ProductId);
            if (product == null)
            {
                AdicionarErro("O produto informado não foi encontrado.");
                return ValidationResult;
            }

            // Validar seller

            var price = new Price(request.ProductId, request.SellerId, request.Value, request.Active);

            await _productRep.AddPriceToProduct(price);

            return await PersistirDados(_productRep.UnitOfWork);
        }
    }
}
