using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModel
{
    public class ViewModelBillInsert
    {
        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Range(1, double.MaxValue, ErrorMessage = "Value bigger than 1")]
        [DisplayFormat(DataFormatString = "{0,c}")]
        public decimal ValueOriginal { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        public virtual Bill ViewModelBillModel()
        {
            return new Bill
            {
                Name = Name,
                ValueOriginal = ValueOriginal,
                DueDate = DueDate,
                PaymentDate = PaymentDate
            };
        }
    }

    public class ViewModelBillUpdate : ViewModelBillInsert
    {
        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Range(1, int.MaxValue, ErrorMessage = "Value bigger than 1")]
        public int Id { get; set; }

        public override Bill ViewModelBillModel()
        {
            return new Bill
            {
                BillId = Id,
                Name = Name,
                ValueOriginal = ValueOriginal,
                DueDate = DueDate,
                PaymentDate = PaymentDate
            };
        }
    }

    public class ViewModelBillDelete : ViewModelBillUpdate { }


    public class ViewModelBillView
    { 
        public int Id { get; set; }

        public string Name { get; set; }
      
        public decimal ValueOriginal { get; set; }

        public decimal ValueCorrected { get; set; }

        public int DelayedDays { get; set; }

        public DateTime PaymentDate { get; set; }

        public ViewModelBillView() { }

        public ViewModelBillView(Bill model, int delayDays, decimal valueCorrected)
        {
            Id = model.BillId;
            Name = model.Name;
            ValueOriginal = model.ValueOriginal;
            PaymentDate = model.PaymentDate;
            DelayedDays = delayDays;
            ValueCorrected = valueCorrected;
        }
    }

}
