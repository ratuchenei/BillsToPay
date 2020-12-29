using Microsoft.EntityFrameworkCore;
using Models;
using System.Threading.Tasks;

namespace Repository
{
    public class InterestRuleRepository : GenericRepository<InterestRule>, IInterestRuleRepository
    {
        public InterestRuleRepository(DatabaseSettings settings) : base(settings)
        {
        }

        public async Task<InterestRule[]> GetAll()
        {
            using (var context = GetContext())
            {
                return await context.Interest.ToArrayAsync();
            }
        }
    }
}
