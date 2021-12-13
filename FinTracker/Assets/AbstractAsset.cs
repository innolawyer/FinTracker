using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Assets.FVCalc;

namespace FinTracker.Assets
{
    public class AbstractAsset
    {
        protected IFVCalcer calcer { get; set; }

        public double GetFutureValue(double amount, int term, double interest)
        {
            return calcer.GetFutureValue(amount, term, interest);
        }
    }
}
