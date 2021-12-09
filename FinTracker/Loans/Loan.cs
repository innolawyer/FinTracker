using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class Loan
    {        

        public DateTime PreviousPaymentDateTime { get; set;}        
        public DateTime ActualPaymentDateTime { get; set; }
        public DateTime NextPaymentDateTime { get; set;}      //нужно ли?  
        public DateTime LastPaymentDateTime { get; set;}        
        public Asset Asset { get; set; }
        public int Id { get; set; }
        public string CreditorsName { get; set; }
        public double Percent { get; set; }
        public double Period { get; set; }
        public string Status { get; set; }        
        public double Amount { get; set; }
        public double TotalAmountOfPercents { get; set; }
        public double TotalAmountOfLoan { get; set; }
        public double RemainingAmountOfPercents { get; set; }
        public double AmountOfReturned { get; set; }        
        public double RemainingTerm { get; set; }
        public double RemainingAmount { get; set; }
        public double MonthlyPayment { get; set; }
        public double MonthlyPaymentRounded { get; set; }
        public double TotalAmountOfExtraPaymentsDoneBetweenDates { get; set; }
        public double TotalAmountOfExtraPaymentsDoneInDateOfPayment { get; set; }
        

        public Loan (Asset asset, int id, DateTime actualPaymentDateTime, string creditorsName,
                    double percent, double period, string status, 
                    double remainingTerm,
                    double amount)
        {
            Asset = asset;
            Id = id;
            PreviousPaymentDateTime = actualPaymentDateTime.AddMonths(-1);
            ActualPaymentDateTime = actualPaymentDateTime;
            NextPaymentDateTime = actualPaymentDateTime.AddMonths(1);
            LastPaymentDateTime = actualPaymentDateTime.AddMonths(Convert.ToInt32(period));
            CreditorsName = creditorsName;
            Percent = percent;
            Period = period;
            Status = status;
            RemainingTerm = remainingTerm;            
            Amount = amount;
            TotalAmountOfPercents = Amount * ((Percent / 1200) * Period);
            TotalAmountOfLoan = Amount + TotalAmountOfPercents;
            AmountOfReturned = 0;
            RemainingAmount = Amount - AmountOfReturned;
            RemainingAmountOfPercents = RemainingAmount * (Percent / 100);
            MonthlyPayment = Amount * ((Percent/1200) / (1-Math.Pow((1+(Percent/1200)), -Period)));
            MonthlyPaymentRounded = Math.Round(MonthlyPayment, 2);
            TotalAmountOfExtraPaymentsDoneBetweenDates = 0;
            TotalAmountOfExtraPaymentsDoneInDateOfPayment = 0;
        }

        //привязать актуальную дату к программе
        public void DoRegularPayment ()
        {          

            while (ActualPaymentDateTime != LastPaymentDateTime)
            {
                if (DateTime.Today == ActualPaymentDateTime)
                {
                    Asset.Amount -= MonthlyPayment;
                    TotalAmountOfLoan -= MonthlyPayment;
                    Amount -= (MonthlyPayment - (Amount * (Percent / 1200)));
                }
            }    
            
        }


        //метод досрочного погашения, уменьшающий ежемесячный платёж
        public void DoExtraPaymentToDecreasePayment (DateTime extraPaymentDate, double extraPaymentAmount, string extraPaymentPurpose)
        {
            double daylyPercent = (Percent / 1200) / (ActualPaymentDateTime-PreviousPaymentDateTime).TotalDays; //вычисляем дневную процентную ставку в текущем месячном промежутке
            
            if (extraPaymentDate != ActualPaymentDateTime)
            {
                if (extraPaymentAmount <= RemainingAmount && RemainingAmount > 0)
                {
                    double balanceForRepaymentOfLoanPercents = RemainingAmount * (((extraPaymentDate - ActualPaymentDateTime).TotalDays) * daylyPercent); //сколько из досрочного погашения уйдет на проценты
                    double balanceForRepaymentOfLoanBody = extraPaymentAmount - balanceForRepaymentOfLoanPercents; //сколько из досрочного погашения уйдет на тело кредита
                    AmountOfReturned += balanceForRepaymentOfLoanBody;
                    Asset.Amount -= MonthlyPayment;
                    MonthlyPayment = (Amount - balanceForRepaymentOfLoanBody) * ((Percent / 1200) / (1 - Math.Pow((1 + (Percent / 1200)), -Period)));
                }
            }
            else
            {
                if (extraPaymentAmount <= RemainingAmount && RemainingAmount > 0)
                {
                    AmountOfReturned += extraPaymentAmount;
                    Asset.Amount -= MonthlyPayment;
                    MonthlyPayment = (Amount - extraPaymentAmount) * ((Percent / 1200) / (1 - Math.Pow((1 + (Percent / 1200)), -Period)));
                }
            }
        }



    }
}
