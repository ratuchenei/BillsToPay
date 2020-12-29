using Models;
using System.Threading.Tasks;

namespace Repository
{
    public interface IGenericRepository<T> : IBaseRepository where T : Base
    {
        Task InsertAsync(params T[] obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(params T[] obj);
    }
}
