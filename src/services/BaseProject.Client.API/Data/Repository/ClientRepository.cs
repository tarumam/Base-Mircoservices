using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Clients.API.Models;
using BaseProject.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Clients.API.Data.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ClientsContext _context;

        public ClientRepository(ClientsContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _context.Clients.AsNoTracking().ToListAsync();
        }

        public Task<Client> GetByCPF(string cpf)
        {
            return _context.Clients.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
        }

        public void Add(Client client)
        {
            _context.Clients.Add(client);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
