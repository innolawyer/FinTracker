using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class Asset
    {
        MainWindow _mainWindow;
        public string Name; //Название
        public double Amount; //Состояние счета
        public double YearInterest; //Процент на остаток // узать как считаеться
        public List<string> CashbackCategories; //категории с повышенным кэшбэком
        public double FixCashback;
        public double ServiceFee;
        public List<Transaction> Transactions = new List<Transaction>();

        private double _StartAmount; //Состояние счета

        public Asset(string name, double amount) // конструктор для бумагжных денег
        {
            Name = name;
            Amount = amount;
            _StartAmount = amount;
        }

        public double GetAmount()
        {
            double result = _StartAmount;
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Sign == "+")
                {
                    result += transaction.Amount;
                }
                else if (transaction.Sign == "-")
                {
                    result -= transaction.Amount;
                }
            }
            return result;
        }

        public void Send(double amount, Asset sender)
        {

        }

        public void EditTransaction(Transaction transaction)
        {

        }

        public void AddTransactions(Transaction nTransaction)
        {
            Transactions.Add(nTransaction);
            Amount = GetAmount();
        }
    }
}
