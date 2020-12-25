using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Core.Data;

namespace BaseProject.Catalog.Domain
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAll(int pageSize, int pageIndex, string query = null);
        Task<Product> GetById(Guid id);
        Task<Product> GetByBarcode(string barcode);
        Task<Product> GetByName(string name);
        void Add(Product produto);
        void Update(Product produto);
        Task<Price> GetCurrentPriceForSeller(Guid productId, Guid sellerId);
        Task<List<Price>> GetPrices(Guid productId);
        Task<Product> GetProductWithPrices(Guid productId);
        Task AddPriceToProduct(Price price);
    }
}
