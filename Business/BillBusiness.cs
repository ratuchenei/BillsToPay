using Models;
using Models.ViewModel;
using Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public class BillBusiness : GenericBusiness<Bill>, IBillBusiness
    {
        private readonly IBillRepository _billRepository;
        private readonly IInterestRuleRepository _interestRuleRepository;

        public BillBusiness(IBillRepository billRepository, IInterestRuleRepository interestRuleRepository) : base(billRepository)
        {
            _billRepository = billRepository;
            _interestRuleRepository = interestRuleRepository;
        }

        public async Task<IEnumerable<ViewModelBillView>> GetAll()
        {
            var models = await _billRepository.GetAll();

            var interest = await _interestRuleRepository.GetAll();

            var view = new List<ViewModelBillView>();

            foreach(var item in models)
            {
                (int delayDays, decimal valueCorrected) = InterestCalculation(item, interest);

                view.Add(new ViewModelBillView(item, delayDays, valueCorrected));
            }

            return view;
        }

        public async Task<ViewModelBillView> InsertBillAsync(ViewModelBillInsert bill)
        {
            var model = bill.ViewModelBillModel();

            await _billRepository.InsertAsync(model);

            var interest = await _interestRuleRepository.GetAll();

            (int delayDays, decimal valueCorrected) = InterestCalculation(model, interest);

            var view = new ViewModelBillView(model, delayDays, valueCorrected);

            return view;
        }


        public (int, decimal) InterestCalculation(Bill item, InterestRule[] rules)
        {
            var delayDays = 0;
            var valueCorrected = item.ValueOriginal;

            var diffday = (item.PaymentDate - item.DueDate).Days;

            if (diffday > 0)
            {
                int rule = (int)((diffday <= 3)
                    ? ERule.AteTresDias : (diffday > 3 && diffday < 6)
                    ? ERule.SuperiorTresDias : ERule.SuperiorCincoDias);

                var interest = rules.Where(x => x.InterestRuleId == rule).FirstOrDefault();

                var valuePenality = decimal.Multiply(item.ValueOriginal, interest.Penalty);
                var delay = decimal.Multiply(diffday, interest.InterestPerDay);
                var valueDelay = decimal.Multiply(item.ValueOriginal, delay);

                valueCorrected += decimal.Add(valuePenality, valueDelay);
                delayDays = diffday;
            }

            return (delayDays, valueCorrected);
        }
    }
}
