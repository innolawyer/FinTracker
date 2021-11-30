using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    internal class Asset
    {
        public string Name; //Название
        public double Amount; //Состояние счета
        public double YearInterest; //Процент на остаток // узать как считаеться
        public List<string> CashbackCategories; //категории с повышенным кэшбэком
        public double FixCashback;
        public double ServiceFee;
        public List<Transaction> Transactions = new List<Transaction>();

        public Asset(string name, double amount) // конструктор для бумагжных денег
        {
            Name = name;
            Amount = amount;
        }

        private void GetAmount() // должен вернуть дабл
        {

        }

        private void Send(Asset reciver) // Отправляет деньги на другой счет
        {

        }
    }
}
