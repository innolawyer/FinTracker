using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.Assets.FVCalc
{
    public interface IFVCalcer
    {
        public double GetFutureValue(double amount, int term, double interest);
    }
}
