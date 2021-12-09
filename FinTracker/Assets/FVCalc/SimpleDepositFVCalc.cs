using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Assets.FVCalc
{
    internal class SimpleDepositFVCalc : IFVCalcer
    {
        public double GetFutureValue(double amount, int term, double interest)
        {
            double result = (amount * interest * (term / 12) / 100);
            return result + amount;
        }
    }
}