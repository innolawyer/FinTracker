using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class Loan
    {        

        public DateTime NextPaymentDateTime { get; set; }
        public Asset Asset { get; set; }
        public string CreditorsName { get; set; }
        public double Percent { get; set; }
        public double Period { get; set; }
        public string Status { get; set; }
        public double Amount { get; set; }
        public double AmountOfReturned { get; set; }        
        public double RemainingTerm { get; set; }
        public double RemainingAmount { get; set; }
        public double MonthlyPayment { get; set; }

        public Loan (Asset asset, DateTime nextPaymentDateTime, string creditorsName,
                    double percent, double period, string status, 
                    double remainingTerm,
                    double amount, double amountOfReturned)
        {
            Asset = asset;
            NextPaymentDateTime = nextPaymentDateTime;
            CreditorsName = creditorsName;
            Percent = percent;
            Period = period;
            Status = status;
            RemainingTerm = remainingTerm;            
            Amount = amount;
            AmountOfReturned = amountOfReturned;
            RemainingAmount = Amount - AmountOfReturned;
            MonthlyPayment = Amount * (((Percent/12)/100) / (1-Math.Pow((1+(Percent/1200)), -Period)));
            MonthlyPayment = Math.Round(MonthlyPayment, 2);
            

        }



    }
}
