using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        public BillRepository(DatabaseSettings settings) : base(settings)
        {
        }

        public async Task<Bill[]> GetAll()
        {
            using (var context = GetContext())
            {
                return await context.Bills.Where(x=>x.IsEnable).ToArrayAsync();
            }
        }
    }
}
