using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class Transaction
    {
        public Storage _storage = Storage.GetStorage();

        public double Amount;
        public string Category;
        public DateTime Date;
        public string Comment;
        public Storage.sign Sign;


        public Transaction(Storage.sign sign,double sum, DateTime date, string comment, string category)
        {
            Sign = sign;
            Amount = sum;
            Date = date;
            Comment = comment;
            Category = category;
        }

        public void EditTransaction(Storage.sign t, double sum, DateTime date, string comment, string category)
        {
            Sign = t;
            Amount = sum;
            Date = date;
            Comment = comment;
            Category = category;
        }
    }
}
