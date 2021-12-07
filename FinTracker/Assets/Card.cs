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
        public DateTime EnrollDateCash; // дата выплаты кэшбека
        public string PeriodEnrollCashbak;

        public double Cashback = 0; // сумма кэшбека 

        public double Percent; // процент кэшбека
        public Dictionary<string, double> CashbackAndPercent = new Dictionary<string, double>(100); //переводы жкх
       
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
            CashbackAndPercent.Add("Перевод", 0);
            CashbackAndPercent.Add("Коммунальные платежи", 0);
        }

        public void AddCategoryCashback(string category, double percent)
        {
            CashbackAndPercent.Add(category, percent);
        }

        public double GetCashbackByCategory(Card asset)
        {
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Sign == "-")
                {
                    foreach (KeyValuePair<string,double> cashback in CashbackAndPercent)
                    {
                        if (transaction.Category == cashback.Key)
                        {
                            Cashback += (transaction.Amount * asset.Percent);
                        }
                        else
                        {
                            Cashback += (transaction.Amount * asset.Percent);
                        }
                    }
                }
            }
            return Cashback;
        }

        public void EnrollmentCashbak()
        {
            if (PeriodEnrollCashbak == "год")
            {
                int god = 0;
                if (DateTime.Today == (EnrollDateCash.AddYears(god)))
                {
                    Amount += Cashback;
                    god++;
                }
            }
            if (PeriodEnrollCashbak == "месяц")
            {
                int moth = 0;
                if (DateTime.Today == (EnrollDateCash.AddMonths(moth)))
                {
                    Amount += Cashback;
                    moth++;
                }
            }
            // и должно быть: MessageBox.Show("За период ДАТА - ДАТА Вам начислен кэшбек за покупки в сумме $"{Cashback + CashbackAll}")
        }
    }
}

