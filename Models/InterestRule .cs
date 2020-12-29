using System;

namespace Models
{
    public class InterestRule : Base
    {
        public int InterestRuleId { get; set; }

        public int DelayDays { get; set; }

        public Decimal Penalty { get; set; }

        public Decimal InterestPerDay { get; set; }
    }
}
