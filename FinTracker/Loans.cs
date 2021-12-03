using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    internal class Loans
    {
        MainWindow mainWindow;

        public DateTime LoanDateTime;
        public string CreditorsName;
        public double LoanPercent;
        public double LoanPeriod;
        public bool LoanStatus;
        public double LoanAmount;
        public double LoanAmountOfReturned;

        public Loans (DateTime loanDateTime, string creditorsName, double loanPercent, double loanPeriod, bool loanStatus, double loanAmount, double loanAmountOfReturned)
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
