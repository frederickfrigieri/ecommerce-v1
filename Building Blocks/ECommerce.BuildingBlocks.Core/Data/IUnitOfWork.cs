using System.Threading.Tasks;

namespace ECommerce.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
