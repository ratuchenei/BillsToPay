using Models;
using System.Threading.Tasks;

namespace Business
{
    public interface IGenericBusiness<T> : IBaseBusiness where T : Base
    {
        Task InsertAsync(params T[] obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(params T[] obj);
    }
}
