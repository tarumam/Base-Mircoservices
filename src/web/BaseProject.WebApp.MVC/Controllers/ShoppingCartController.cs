using System;
using System.Threading.Tasks;
using BaseProject.WebApp.MVC.Models;
using BaseProject.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.WebApp.MVC.Controllers
{
    [Authorize]
    public class ShoppingCartController : MainController
    {
        private readonly IShoppingCartService _shoppingCartSvc;
        private readonly ICatalogService _catalogSvc;

        public ShoppingCartController(IShoppingCartService carrinhoService,
                                  ICatalogService catalogoService)
        {
            _shoppingCartSvc = carrinhoService;
            _catalogSvc = catalogoService;
        }

        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _shoppingCartSvc.GetShoppingCart());
        }

        [HttpPost, Route("carrinho/adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ProductItemViewModel itemProduto)
        {
            var produto = await _catalogSvc.GetById(itemProduto.ProductId);

            ValidateItemInShoppingCart(produto, itemProduto.Amount);
            if (!IsValidOperation()) return View("Index", await _shoppingCartSvc.GetShoppingCart());

            itemProduto.Name = produto.Nome;
            itemProduto.Value = produto.Valor;
            itemProduto.Image = produto.Imagem;

            var resposta = await _shoppingCartSvc.AddItemToShoppingCart(itemProduto);

            if (ResponseHasErrors(resposta)) return View("Index", await _shoppingCartSvc.GetShoppingCart());

            return RedirectToAction("Index");
        }

        [HttpPost, Route("carrinho/atualizar-item")]
        public async Task<IActionResult> UpdateItemInShoppingCart(Guid produtoId, int quantidade)
        {
            var produto = await _catalogSvc.GetById(produtoId);

            ValidateItemInShoppingCart(produto, quantidade);
            if (!IsValidOperation()) return View("Index", await _shoppingCartSvc.GetShoppingCart());

            var itemProduto = new ProductItemViewModel { ProductId = produtoId, Amount = quantidade };
            var resposta = await _shoppingCartSvc.UpdateItemInShoppingCart (produtoId, itemProduto);

            if (ResponseHasErrors(resposta)) return View("Index", await _shoppingCartSvc.GetShoppingCart());

            return RedirectToAction("Index");
        }

        [HttpPost, Route("carrinho/remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogSvc.GetById(produtoId);

            if (produto == null)
            {
                AddValidationError("Produto inexistente!");
                return View("Index", await _shoppingCartSvc.GetShoppingCart());
            }

            var resposta = await _shoppingCartSvc.RemoveItemFromShoppingCart(produtoId);

            if (ResponseHasErrors(resposta)) return View("Index", await _shoppingCartSvc.GetShoppingCart());

            return RedirectToAction("Index");
        }

        private void ValidateItemInShoppingCart(ProductViewModel produto, int quantidade)
        {
            if (produto == null) AddValidationError("Produto inexistente!");
            if (quantidade < 1) AddValidationError($"Escolha ao menos uma unidade do produto {produto.Nome}");
            if (quantidade > produto.QuantidadeEstoque) AddValidationError($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
        }
    }
}
