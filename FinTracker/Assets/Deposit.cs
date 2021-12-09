using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Assets.FVCalc;

namespace FinTracker
{
    internal class Deposit : Asset
    {
        public bool Withdrawable;
        public bool Putable;
        public bool Сapitalization;
        public DateTime ClosingDate;

        public Deposit(string name, double amount, bool withdrawable, bool putable, bool capitalization, DateTime closingDate) : base(name, amount)
        {
            if (capitalization)
            {
                calcer = new CompoundDepositFVCalc();
            }
            else
            {
                calcer = new SimpleDepositFVCalc();
            }
            Name = name;
            Amount = amount;    
            Withdrawable = withdrawable;
            Putable = putable;
            Сapitalization = capitalization;
            ClosingDate = closingDate;
        }


    }
}
