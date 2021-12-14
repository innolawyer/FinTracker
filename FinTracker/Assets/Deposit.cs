using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinTracker.Assets.FVCalc;

namespace FinTracker
{
    internal class Deposit : Asset
    {
        public bool Withdrawable; // возможность снятия
        public bool Putable;  // возможность внесения
        public bool Сapitalization; // процент к сумме вклада или нет
        public DateTime ClosingDate; // дата закрытия
        public DateTime OpeningDate; // дата открытия
        public double Percent; // годовой процент
        public double SumIncome; // сумма начисления
        public Storage.period Period; // период
        public DateTime SpendDate; // дата зачисления

        public Deposit(string name, double amount, bool withdrawable, bool putable, bool capitalization, DateTime closingDate, DateTime openingDate, double percent, Storage.period period) : base(name, amount)
        {
            if (capitalization)
            {
                calcer = new CompoundDepositFVCalc();
            }
            else
            {
                calcer = new SimpleDepositFVCalc();
            }
            Name = name;
            Amount = amount;
            Withdrawable = withdrawable;
            Putable = putable;
            Сapitalization = capitalization;
            ClosingDate = closingDate;
            OpeningDate = openingDate;
            Percent = percent;
            Period = period;
            SpendDate = openingDate.AddDays((int)Period * 360);
        }

        // если % добавляется к вкладу, расчет на 1 период, этот нельзя снимать, можно пополнять
        public void EnrollIncomeFromDepositHardPercent()
        {
            if (DateTime.Today >= SpendDate)
            {
                Amount += Amount * (Percent * (double)Period);
                SpendDate = SpendDate.AddDays((int)Period * 360);
            }
        }

        // если % не добавляется к вкладу, расчет на 1 период, это можно снимать и пополнять
        public void EnrollIncomeFromDepositSimplePercent()
        {
            if (DateTime.Today >= SpendDate)
            {
                SumIncome += Amount * (Percent * (double)Period);
                SpendDate = SpendDate.AddDays((int)Period * 360);
            }
        }

        // пополнение, если возможность поплнения тру
        public void SpendDeposit(double sum)
        {
            if (Putable == true)
            {
                Amount += sum;
            }
        }

        // снятие, если возможность снятия тру
        public void IncomeDeposit(double sum)
        {
            if(Withdrawable == true)
            {
                Amount -= sum;
            }
        }



    }
}
