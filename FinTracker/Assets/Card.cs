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
        public double SumYearInterest;
        public string PeriodEnrollSumYearInterest;
        public DateTime EnrollDateYearInterest;
        public List<string> CashbackCategories; //категории с повышенным кэшбэком
        public double FixCashback;
        public double ServiceFee;
        public double MinAmount;
        public DateTime EnrollDateCash; // дата выплаты кэшбека
        public string PeriodEnrollCashbak;

        public double Cashback = 0; // сумма кэшбека 

        public double Percent; // процент кэшбека
        public Dictionary<string, double> CashbackAndPercent = new Dictionary<string, double>(); //переводы жкх
       
        public Card(string name, double amount, double yearInterest, double fixCashback, double serviceFee, DateTime enrollDateCash, string periodEnrollCashbak ) : base(name, amount)
        {
            Name = name;
            Amount = amount;
            MinAmount = amount;
            ServiceFee = serviceFee; // год или месяц?
            YearInterest = yearInterest /100;
            FixCashback = fixCashback;
            EnrollDateCash = enrollDateCash;
            PeriodEnrollCashbak = periodEnrollCashbak;
            CashbackAndPercent.Add("Перевод", 0);
            CashbackAndPercent.Add("Коммунальные платежи", 0);
        }

        public void AddCategoryCashback(string category, double percent)
        {
            CashbackAndPercent.Add(category, percent / 100);
        }

        public void DeleteCategoryCashBack(string category)
        {
            CashbackAndPercent.Remove(category);
        }

        public double GetMinAmount()
        {
            double tmpAmount = GetAmount();
            if (tmpAmount < MinAmount)
            {
                MinAmount = tmpAmount;
            }
            if (DateTime.Today == EnrollDateYearInterest)
            {
                MinAmount = GetAmount();
            }
            return MinAmount;
        }

        public double GetCashback(Card asset)
        {
            foreach (Transaction transaction in Transactions)
            {
                if (transaction.Date >= new DateTime (EnrollDateCash.Year,EnrollDateCash.Month - 1, EnrollDateCash.Day)
                    && transaction.Date < EnrollDateCash)
                {
                    if (transaction.Sign == Storage.sign.spend)
                    {
                        foreach (KeyValuePair<string, double> cashback in CashbackAndPercent)
                        {
                            if (transaction.Category == cashback.Key)
                            {
                                Cashback += (transaction.Amount * cashback.Value);
                            }
                            else
                            {
                                Cashback += (transaction.Amount * asset.FixCashback);
                            }
                        }
                    }
                }
            }
            return Cashback;
        }

        public double  GetSumYearInterest()
        {
            SumYearInterest = MinAmount * (YearInterest / 12) ;
            return SumYearInterest;
        }

        public void EnrollmentSumYearInterest()
        {
            if (PeriodEnrollSumYearInterest == "год")
            {
                int year = 0;
                if (DateTime.Today == EnrollDateYearInterest)
                {
                    EnrollDateYearInterest = EnrollDateYearInterest.AddYears(year);
                    Amount += SumYearInterest;
                    SumYearInterest = 0;
                    year++;
                }
            }
            if (PeriodEnrollSumYearInterest == "месяц")
            {
                int month = 0;
                if (DateTime.Today == EnrollDateYearInterest)
                {
                    EnrollDateYearInterest = EnrollDateYearInterest.AddYears(month);
                    Amount += SumYearInterest;
                    SumYearInterest = 0;
                    month++;
                }
            }
        }

        public void EnrollmentCashbak()
        {
            if (PeriodEnrollCashbak == "год")
            {
                int year = 0;
                if (DateTime.Today == EnrollDateCash)
                {
                    EnrollDateCash = EnrollDateCash.AddYears (year);
                    Amount += Cashback;
                    Cashback = 0;
                    year++;
                }
            }
            if (PeriodEnrollCashbak == "месяц")
            {
                int month = 0;
                if (DateTime.Today == EnrollDateCash)
                {
                    EnrollDateCash = EnrollDateCash.AddYears(month);
                    Amount += Cashback;
                    Cashback=0;
                    month++;
                }
            }
            // и должно быть: MessageBox.Show("За период ДАТА - ДАТА Вам начислен кэшбек за покупки в сумме $"{Cashback + CashbackAll}")
        }
    }
}

