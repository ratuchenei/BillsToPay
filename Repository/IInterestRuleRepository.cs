using Models;
using System.Threading.Tasks;

namespace Repository
{
    public interface IInterestRuleRepository : IGenericRepository<InterestRule>
    {
        Task<InterestRule[]> GetAll();
    }
}
