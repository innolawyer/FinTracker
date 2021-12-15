using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Assets;
using FinTracker.Assets.FVCalc;

namespace FinTracker
{
    public class Card : AbstractAsset
    {
        public double YearInterest; //Процент на остаток // узать как считаеться
        public double SumYearInterest;
        public DateTime EnrollDateYearInterest;
        public double MinAmount;      

        public double FixCashback;
        public DateTime EnrollDateCash; // дата выплаты кэшбека

        public double ServiceFee;
        public DateTime DateSpendServiceFee;

        public double Cashback = 0; // сумма кэшбека 

        public double Percent; // процент кэшбека
        public List<string> CashbackCategories; //категории с повышенным кэшбэком
        public Dictionary<string, double> CashbackAndPercent = new Dictionary<string, double>();
       
        public Card (string name, double amount, 
            double yearInterest, double fixCashback, double serviceFee, 
            DateTime enrollDateCash, DateTime enrollDateYearInterest, 
            DateTime dateSpendServiceFee)
        {
            calcer = new CardFVCalc();
            Name = name;
            Amount = amount;
            _StartAmount = amount;
            MinAmount = amount;
            ServiceFee = serviceFee;
            DateSpendServiceFee = dateSpendServiceFee;       
            YearInterest = yearInterest / 100;
            FixCashback = fixCashback / 100;
            EnrollDateCash = enrollDateCash;          
            EnrollDateYearInterest = enrollDateYearInterest;
            CashbackAndPercent.Add("Перевод", 0);
            CashbackAndPercent.Add("Коммунальные платежи", 0);
        }

        public void AddCategoryCashback(string category, double percent)
        {
            CashbackAndPercent.Add(category, percent / 100);
        }

        public double GetMinAmount() //при запуске и в транзакциях
        {
            double tmpAmount = GetAmount();
            if (tmpAmount < MinAmount)
            {
                MinAmount = tmpAmount;
            }
            if (DateTime.Today >= EnrollDateYearInterest)
            {
                MinAmount = GetAmount();
            }
            return MinAmount;
        }

        public double GetCashback()
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
                                Cashback += (transaction.Amount * FixCashback);
                            }
                        }
                    }
                }
            }
            return Cashback;
        }

        public double GetSumYearInterest()
        {
            SumYearInterest = MinAmount * (YearInterest / 12) ;
            return SumYearInterest;
        }

        public void EnrollmentServiceFee()
        {
            if (DateTime.Today >= DateSpendServiceFee)
            {
                Amount -= ServiceFee;
                DateSpendServiceFee = DateSpendServiceFee.AddMonths(1);
            }
        }

        public void EnrollmentSumYearInterest()
        {
            if (DateTime.Today >= EnrollDateYearInterest)
            {
                SumYearInterest =  GetSumYearInterest();
                EnrollDateYearInterest = EnrollDateYearInterest.AddMonths(1);
                Amount += SumYearInterest;
                SumYearInterest = 0;
            }

        }

        public void EnrollmentCashbak()
        {

            if (DateTime.Today >= EnrollDateCash)
            {
                Cashback = GetCashback();
                EnrollDateCash = EnrollDateCash.AddMonths(1);
                Amount += Cashback;
                Cashback = 0;
            }

            // и должно быть: MessageBox.Show("За период ДАТА - ДАТА Вам начислен кэшбек за покупки в сумме $"{Cashback + CashbackAll}")
        }

        public void EditCard(string name, double amount,
            double yearInterest, double fixCashback, double serviceFee,
            DateTime enrollDateCash, DateTime enrollDateYearInterest,
            DateTime dateSpendServiceFee)
        {
            Name = name;
            Amount = amount;       
            ServiceFee = serviceFee;
            DateSpendServiceFee = dateSpendServiceFee;
            YearInterest = yearInterest / 100;
            FixCashback = fixCashback / 100;
            EnrollDateCash = enrollDateCash;
            EnrollDateYearInterest = enrollDateYearInterest;

            if (Transactions == null)
            {
                _StartAmount = amount;
                Amount = _StartAmount;
                MinAmount = amount;
            }
        }
    }
}

