using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Core.Mediatr;
using BaseProject.Seller.API.Application.Commands;
using BaseProject.Seller.API.Data.Repository;
using BaseProject.Seller.API.Models;
using BaseProject.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Seller.API.Controllers
{
    public class SellerController : MainController
    {
        private readonly ISellerRepository _sellerRep;
        private readonly IMediatorHandler _mediatorHandler;

        public SellerController(ISellerRepository prodRepository, IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
            _sellerRep = prodRepository;
        }

        [HttpPost, Route("new-seller")]
        public async Task<IActionResult> AddProduct(SellerModel seller)
        {
            var resultado = await _mediatorHandler.SendCommand(
                new AddSellerCommand(seller.Name, seller.Address, seller.Neighbohood, seller.Number, seller.Reference, seller.WorkingTime, seller.Details, seller.Image, seller.HasParking));

            return CustomResponse(resultado);
        }

        [HttpGet, Route("sellers")]
        public async Task<List<Data.Seller>> GetSellers(int pageIndex = 1, int pageSize = 30)
        {
            var result = await _sellerRep.GetSellers(pageIndex, pageSize);
            return result;
        }

        [HttpGet, Route("sellers-by-name")]
        public async Task<List<Data.Seller>> GetSellersByName(string text, int pageIndex = 1, int pageSize = 30)
        {
            var result = await _sellerRep.GetSellersByName(text, pageIndex, pageSize);
            return result;
        }
    }
}
