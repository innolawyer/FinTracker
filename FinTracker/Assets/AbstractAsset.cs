using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Assets.FVCalc;

namespace FinTracker.Assets
{
    public abstract class AbstractAsset
    {
        protected IFVCalcer calcer { get; set; }

        public string Name; //Название
        public double Amount; //Состояние счета       
        public List<Transaction> Transactions = new List<Transaction>();
        private double _StartAmount; //Стартовое состояние счета

        public AbstractAsset(string name, double amount)
        {
            calcer = new CashFVCalc();
            Name = name;
            Amount = amount;
            _StartAmount = amount;
        }

        public AbstractAsset()
        {

        }


        public double GetAmount()
        {
            double result = _StartAmount;
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Sign == Storage.sign.income)
                {
                    result += transaction.Amount;
                }
                else if (transaction.Sign == Storage.sign.spend)
                {
                    result -= transaction.Amount;
                }
            }
            return result;
        }

        public void AddTransactions(Transaction nTransaction)
        {
            Transactions.Add(nTransaction);
            Amount = GetAmount();
        }

        public void DeleteTransaction(Transaction curTransaction)
        {
            Transactions.Remove(curTransaction);
        }

        public void EditAsset(string name, double amount)
        {
            Name = name;
            Amount = amount;

            if (Transactions == null)
            {
                _StartAmount = amount;
                Amount = _StartAmount;
            }
        }

        public double GetFutureValue(double amount, int term, double interest)
        {
            return calcer.GetFutureValue(amount, term, interest);
        }
    }
}
