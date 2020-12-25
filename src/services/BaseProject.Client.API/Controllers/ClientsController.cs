using System;
using System.Threading.Tasks;
using BaseProject.Clients.API.Application.Commands;
using BaseProject.Core.Mediatr;
using BaseProject.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.Clients.API.Controllers
{
    public class ClientsController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientsController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> Index()
        {
            var resultado = await _mediatorHandler.SendCommand (
                new AddClientCommand(Guid.NewGuid(), "Tarumã", "tttt@fffff.com", "123123"));

            return CustomResponse(resultado);
        }
    }
}
