using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FinTracker.Loans
{
    public class LoanTransaction:Transaction
    {
        public DateTime LoanTransactionDate { get; set; }
        public double LoanTransactionAmount { get; set; }
        public string Purpose { get; set; }
        public LoanTransaction(Storage.sign sign, double sum, DateTime date, string comment, string category, string purpose) : base(sign, sum, date, comment, category)
        {
            LoanTransactionDate = date;
            LoanTransactionAmount = sum;
            Purpose = purpose;
        }

    }
}
