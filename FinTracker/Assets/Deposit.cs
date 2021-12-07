using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    internal class Deposit : Asset
    {
        public Deposit(string name, double amount) : base(name, amount)
        {

        }
    }
}
