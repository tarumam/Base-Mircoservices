using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Catalog.API.Application.Commands;
using BaseProject.Catalog.API.Models;
using BaseProject.Catalog.Domain;
using BaseProject.Core.Mediatr;
using BaseProject.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Catalog.API.Controllers
{
    [Route("[controller]")]
    public class CatalogController : MainController
    {
        private readonly IProductRepository _prodRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public CatalogController(IProductRepository prodRepository, IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
            _prodRepository = prodRepository;
        }

        [HttpGet("products")]
        public async Task<IEnumerable<Product>> GetProducts(int pageSize = 10, int pageIndex = 1)
        {
            return await _prodRepository.GetAll(pageSize, pageIndex);
        }

        [HttpGet("products/{id}")]
        public async Task<Product> ProdutoDetalhe(Guid id)
        {
            return await _prodRepository.GetById(id);
        }

        /// <summary>
        /// Add a new product
        /// </summary>
        /// <param name="product" cref="Product">Product</param>
        /// <returns></returns>
        [HttpPost, Route("new-product")]
        public async Task<IActionResult> AddProduct(ProductModel product)
        {
            var resultado = await _mediatorHandler.SendCommand(
                new AddProductCommand(product.Barcode, product.Name, product.Description, true, product.Image));

            return CustomResponse(resultado);
        }

        /// <summary>
        /// Add price to a product
        /// </summary>
        /// <param name="price" cref="PriceModel">Price</param>
        /// <returns></returns>
        [HttpPost, Route("add-price")]
        public async Task<IActionResult> AddPriceToProduct(PriceModel price)
        {
            var resultado = await _mediatorHandler.SendCommand(
                new AddPriceCommand(price.ProductId, price.SellerId, price.Value));

            return CustomResponse(resultado);
        }
    }
}
