using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Core.Data;

namespace BaseProject.Seller.API.Data.Repository
{
    public interface ISellerRepository : IRepository<Seller>
    {
        void Add(Seller produto);
        void Update(Seller produto);
        Task<List<Seller>> GetSellers(int pageSize, int pageIndex);
        Task<List<Seller>> GetSellersByName(string text, int pageIndex, int pageSize);
        Task<Seller> GetSellerById(Guid sellerId);
    }
}
