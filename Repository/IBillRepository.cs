using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        Task<Bill[]> GetAll();
    }
}
