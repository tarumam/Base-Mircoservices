using System;
using System.Threading.Tasks;
using BaseProject.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebApp.MVC.Controllers
{
    public class CatalogController : MainController
    {
        private readonly ICatalogService _catalogoService;
        public CatalogController(ICatalogService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            var produtos = await _catalogoService.GetAll();

            return View(produtos);
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var produto = await _catalogoService.GetById(id);

            return View(produto);
        }

    }
}
