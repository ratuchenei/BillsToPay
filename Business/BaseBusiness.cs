using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public abstract class BaseBusiness : IBaseBusiness
    {
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
