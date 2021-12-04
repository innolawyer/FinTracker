using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class Transaction
    {
        public double Amount;
        public string Category;
        public DateTime Date;
        public string Comment;
        public string Sign;

        public Transaction(string sign, double sum, DateTime date, string comment, string category)
        {
            Sign = sign;
            Amount = sum;
            Date = date;
            Comment = comment;
            Category = category;
        }

        public void EditTransaction(double sum, DateTime date, string comment, string category)
        {
            Amount = sum;
            Date = date;
            Comment = comment;
            Category = category;
        }
    }
}
