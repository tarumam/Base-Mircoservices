using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseProject.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Seller.API.Data.Repository
{
    public class SellerRepository : ISellerRepository
    {
        private readonly SellerContext _context;

        public SellerRepository(SellerContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async void Add(Seller seller)
        {
            await _context.Sellers.AddAsync(seller);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<Seller> GetSellerById(Guid sellerId)
        {
            return await _context.Sellers.FirstOrDefaultAsync(a => a.Id == sellerId);
        }

        public async Task<List<Seller>> GetSellers(int pageIndex, int pageSize)
        {
            return await _context.Sellers
                .Skip(pageIndex - 1)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Seller>> GetSellersByName(string text, int pageIndex, int pageSize)
        {
            return await _context.Sellers
                .Where(a => a.Name.ToUpper().Contains(text.ToUpper()))
                .Skip(pageIndex - 1)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public void Update(Seller seller)
        {
            _context.Sellers.Update(seller);
        }
    }
}
