using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FinTracker.Loans
{
    public class LoanTransaction:Transaction
    {
        public string Purpose;
        public LoanTransaction(Storage.sign sign, double sum, DateTime date, string comment, string category, string purpose) : base(sign, sum, date, comment, category)
        {
            Purpose = purpose;
        }

    }
}
