using System.Threading.Tasks;

namespace BaseProject.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();

        bool IsDBHelthy();
    }
}
