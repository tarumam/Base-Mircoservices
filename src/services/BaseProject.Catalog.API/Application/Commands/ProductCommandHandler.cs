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
        IRequestHandler<AddPriceCommand, ValidationResult>,
        IRequestHandler<UpdateProductCommand, ValidationResult>
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

            Product product = null;

            if (request.ProductId != Guid.Empty && request.ProductId != null)
            {
                product = await _productRep.GetProductWithPrices((Guid)request.ProductId);
            }
            else if (!string.IsNullOrEmpty(request.Barcode))
            {
                product = await _productRep.GetByBarcode(request.Barcode);
            }
            else
            {
                AdicionarErro("A identificação do produto não foi informada.");
                return ValidationResult;
            }

            if (product == null)
            {
                product = new Product(request.Barcode, "Buscando informações", null, true, null);
                _productRep.Add(product);
            }

            var price = new Price(product.Id, request.SellerId, request.Value, request.Active);

            await _productRep.AddPriceToProduct(price);

            return await PersistirDados(_productRep.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return request.ValidationResult;

            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Barcode))
            {
                AdicionarErro("As informações do produto são inválidas.");
                return ValidationResult;
            }

            var existentProduct = await _productRep.GetById(request.Id);
            if (existentProduct.Barcode != request.Barcode)
            {
                AdicionarErro("O produto informado é inválido, verifique o código de barras.");
            }

            existentProduct.SetName(request.Name);
            existentProduct.setImage(request.Image);
            existentProduct.SetDescription(request.Description);
            existentProduct.setActive(request.Active);
            existentProduct.SetSyncWithWeb(request.SyncWithWeb);

            _productRep.Update(existentProduct);

            existentProduct.AddEvent(new ProductAddedEvent(existentProduct.Id, existentProduct.Barcode, existentProduct.Name, existentProduct.Description, existentProduct.Active, existentProduct.CreatedAt, existentProduct.Image));

            return await PersistirDados(_productRep.UnitOfWork);
        }
    }
}
