using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Assets.FVCalc
{
    internal class CompoundDepositFVCalc : IFVCalcer
    {
        public double GetFutureValue(double amount, int term, double interest)
        {
            double result = (amount * Math.Pow((1 + interest), term));
            return result + amount;
        }
    }
}
