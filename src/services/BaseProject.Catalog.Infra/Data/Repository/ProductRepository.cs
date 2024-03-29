﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProject.Catalog.Domain;
using BaseProject.Core.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Catalog.Infra.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ProductRepository(CatalogContext context)
        {
            _context = context;
        }

        public async void Add(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task AddPriceToProduct(Price price)
        {
            var lastPrice = await _context.Prices.Where(s => s.ProductId == price.ProductId
                    && s.CreatedAt == _context.Prices.Max(x => x.CreatedAt)
                    && s.SellerId == price.SellerId)
                .FirstOrDefaultAsync();
            lastPrice?.SetActive(false);
            await _context.Prices.AddAsync(price);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<Product>> GetAll(int pageSize, int pageIndex, string query = null)
        {
            var result = await _context.Products.Include(p =>
                p.Prices.Where(pr => pr.Active == true))
                .OrderBy(a=>a.Name)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();
            return result;
        }

        public async Task<Product> GetByBarcode(string barcode)
        {
            var product = await _context.Products
                .Where(a => a.Barcode == barcode)
                .FirstOrDefaultAsync();
            return product;
        }

        public async Task<Product> GetById(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> GetByName(string name)
        {
            return await _context.Products
                .Where(a => a.Name.ToUpperInvariant().Contains(name.ToUpperInvariant()))
                .FirstOrDefaultAsync();
        }

        public async Task<Price> GetCurrentPriceForSeller(Guid productId, Guid sellerId)
        {
            var price = await _context.Prices.Where(a =>
                a.ProductId == productId
                && a.SellerId == sellerId
                && a.Active == true)
                .FirstOrDefaultAsync();

            return price;
        }

        public async Task<List<Price>> GetPrices(Guid productId)
        {
            var prices = await _context.Prices
                .Where(a => a.ProductId == productId && a.Active == true).ToListAsync();
            return prices;
        }

        public async Task<Product> GetProductWithPrices(Guid productId)
        {
            var prices = _context.Products
                                .Include(e => e.Prices)
                                .Where(a => a.Id == productId);

            return await prices.FirstOrDefaultAsync();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<List<Product>> GetProductsWithPendingBaseInfo()
        {
            var barcodes = await _context.Products
                  .Where(a => a.SyncWithWeb == false)
                  .OrderBy(a=>a.CreatedAt)
                  .ToListAsync();

            return barcodes;
        }

        public async Task<List<BluesoftToken>> GetBluesoftValidToken()
        {
            return await _context.BluesoftTokens
                .Where(a => a.Executions <= 25 && a.Executions > 0)
                .ToListAsync();
        }

        public async void SetExecuteValueToken(BluesoftToken token)
        {
            _context.BluesoftTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetByName(int pageSize, int pageIndex, string text)
        {
            var result = await _context.Products.Include(p =>
                p.Prices.Where(pr => pr.Active == true))
                .Where(a => a.Name.ToUpper().Contains(text.ToUpper()))
                .OrderBy(a => a.Name)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return result;
        }
    }
}
