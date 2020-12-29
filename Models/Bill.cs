using System;

namespace Models
{
    public class Bill : Base
    {
        public int BillId { get; set; }

        public string Name { get; set; }

        public decimal ValueOriginal { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
