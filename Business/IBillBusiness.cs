using Models;
using Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business
{
    public interface IBillBusiness: IGenericBusiness<Bill>
    {
        Task<ViewModelBillView> InsertBillAsync(ViewModelBillInsert bill);
        Task<IEnumerable<ViewModelBillView>> GetAll();
        (int, decimal) InterestCalculation(Bill item, InterestRule[] rules);
    }
}
