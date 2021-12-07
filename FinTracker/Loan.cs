using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class Loan
    {        

        public DateTime DateTime { get; set; }
        public string CreditorsName { get; set; }
        public double Percent { get; set; }
        public double Period { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public double AmountOfReturned { get; set; }        
        public double RemainingTerm { get; set; }
        public double RemainingAmount { get; set; }

        public Loan (DateTime dateTime, string creditorsName,
                    double percent, double period, string status, 
                    double remainingTerm,
                    double amount, double amountOfReturned)
        {
            DateTime = dateTime;
            CreditorsName = creditorsName;
            Percent = percent;
            Period = period;
            Status = status;
            RemainingTerm = remainingTerm;            
            Amount = amount;
            AmountOfReturned = amountOfReturned;
            RemainingAmount = Amount - AmountOfReturned ;

        }



    }
}
