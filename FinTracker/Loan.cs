using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class Loan
    {
        MainWindow mainWindow;

        public DateTime LoanDateTime;
        public string CreditorsName;
        public double LoanPercent;
        public double LoanPeriod;
        public string LoanStatus;
        public double LoanAmount;
        public double LoanAmountOfReturned;

        public Loan (DateTime loanDateTime, string creditorsName,
                    double loanPercent, double loanPeriod, string loanStatus,
                    double loanAmount, double loanAmountOfReturned)
        {
            LoanDateTime = loanDateTime;
            CreditorsName = creditorsName;
            LoanPercent = loanPercent;
            LoanPeriod = loanPeriod;
            LoanStatus = loanStatus;
            LoanAmount = loanAmount;
            LoanAmountOfReturned = loanAmountOfReturned;            

        }

    }
}
