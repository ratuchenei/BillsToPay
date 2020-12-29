using Business;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using System.Threading.Tasks;

namespace BillsToPay.Controllers
{
    [Produces("application/json")]
    [Route("api/Bills")]
    public class BillsController : ControllerBase
    {
        private readonly IBillBusiness _billBusiness;

        public BillsController(IBillBusiness billBusiness)
        {
            _billBusiness = billBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ViewModelBillInsert bill)
        {
            if (!ModelState.IsValid)             
                return BadRequest(ModelState);

            var view = await _billBusiness.InsertBillAsync(bill);

            return Ok(view);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var view = await _billBusiness.GetAll();

            return Ok(view);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ViewModelBillUpdate bill)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _billBusiness.UpdateAsync(bill.ViewModelBillModel());

            return Ok(bill);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] ViewModelBillDelete bill)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _billBusiness.DeleteAsync(bill.ViewModelBillModel());

            return Ok(bill);
        }

    }
}
