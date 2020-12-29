using System;

namespace Models
{
    public abstract class Base
    {
        public DateTime EntryDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public bool IsEnable { get; set; }

        protected Base()
        {
            IsEnable = true;
            EntryDateTime = DateTime.UtcNow;
            UpdateDateTime = DateTime.UtcNow;
        }
    }
}
