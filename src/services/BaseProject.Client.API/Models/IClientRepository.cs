
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProject.Core.Data;

namespace BaseProject.Clients.API.Models
{
    public interface IClientRepository : IRepository<Client>
    {
        void Add(Client cliente);
        Task<IEnumerable<Client>> GetAll();
        Task<Client> GetByCPF(string cpf);
    }
}
