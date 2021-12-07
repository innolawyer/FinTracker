using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    internal class Card : Asset
    {
        public double YearInterest; //Процент на остаток // узать как считаеться
        public List<string> CashbackCategories; //категории с повышенным кэшбэком
        public double FixCashback;
        public double ServiceFee;

        public double MinAmount;

        public double Cashback = 0; // сумма кэшбека по категориям
        public double CashbackAll = 0; // сумма кэшбека на все

        public double Percent; // процент кэшбека
        public DateTime EnrollDateCash; // дата выплаты кэшбека
        public string PeriodEnrollCashbak;
        Dictionary<string, double> CashbackAndPercent = new Dictionary<string, double>(100); //переводы жкх
       
        public Card(string name, double amount, double yearInterest, double fixCashback, double serviceFee, double percent, DateTime enrollDateCash, string periodEnrollCashbak ) : base(name, amount)
        {
            Name = name;
            Amount = amount;
            MinAmount = amount;
            ServiceFee = serviceFee; // год или месяц?
            YearInterest = yearInterest;
            FixCashback = fixCashback;
            EnrollDateCash = enrollDateCash;
            PeriodEnrollCashbak = periodEnrollCashbak;
        }
        
        public double GetCashbackByCategory(Card asset)
        {
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Sign == "+")
                {
                    foreach (string cashback in CashbackCategories)
                    {
                        if (transaction.Category == cashback)
                        {
                            Cashback += (transaction.Amount * asset.Percent);
                        }
                    }
                }
            }
            return Cashback;
        }
        public double GetCashbackAll(Card asset)
        {
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Sign == "+")
                {
                    CashbackAll += (transaction.Amount * asset.Percent);
                }
            }
            return CashbackAll;

        }
        public void EnrollmentCashbak()
        {
            if (PeriodEnrollCashbak == "год")
            {
                int god = 0;
                if (DateTime.Today == (EnrollDateCash.AddYears(god)))
                {
                    Amount += Cashback + CashbackAll;
                    god++;
                }
            }
            if (PeriodEnrollCashbak == "месяц")
            {
                int moth = 0;
                if (DateTime.Today == (EnrollDateCash.AddMonths(moth)))
                {
                    Amount += Cashback + CashbackAll;
                    moth++;
                }
            }
            // и должно быть: MessageBox.Show("За период ДАТА - ДАТА Вам начислен кэшбек за покупки в сумме $"{Cashback + CashbackAll}")
        }
    }
}
}
