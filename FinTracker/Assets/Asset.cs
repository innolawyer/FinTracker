using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Assets;
using FinTracker.Assets.FVCalc;

namespace FinTracker
{
    public class Asset : AbstractAsset
    {

        public AbstractAsset(string name, double amount)
        {
            calcer = new CashFVCalc();
            Name = name;
            Amount = amount;
            _StartAmount = amount;
        }
    }
}
