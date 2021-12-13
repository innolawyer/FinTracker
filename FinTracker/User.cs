using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class User
    {
        public string Name;
        public List<string> CategoriesSpend;
        public List<string> CategoriesIncome;
        public List<Asset> Assets = new List<Asset>();
        public List<Loan> Loans = new List<Loan>();

        public User(string name) 
        {
            Name = name;
            CategoriesSpend = new List<string>() { "Супермаркет", "Транспорт", "Коммунальные платежи", "Снятия",
            "Одежда и обувь", "Рестораны", "Отдых и развлечения", "Здоровье и красота", "Комиссия", "Онлайн сервисы и подписки",
            "Связь и интернет", "Дом и ремонт", "Животные", "Перевод", "Подарки", "Прочие расходы" };
            CategoriesIncome = new List<string>() { "Зарплата", "Начисление кэшбека", "Начисление % по вкладам", "Перевод",
            "Подарок", "Случайный доход" };
        }

        public void AddAsset(string name, double startAmount)
        {
            Assets.Add(new Asset(name, startAmount)); 
        }

        public void AddCard(string name, double startAmount, double yearInterest, double fixCashback, double serviceFee,
            DateTime enrollDateCash, DateTime enrollDateYearInterest, DateTime dateSpendServiceFee)
        {
            Assets.Add(new Card(name, startAmount, yearInterest, fixCashback, serviceFee, enrollDateCash, 
                     enrollDateYearInterest, dateSpendServiceFee));
        }

        public void DeleteAsset(Asset asset)
        {
            Assets.Remove(asset);
        }

        public Asset GetAssetByName(string name)
        {
            foreach (Asset asset in Assets)
            {
                if (asset.Name == name)
                {
                    return asset;
                }
            }
            return null; //отбить это говно (так быть не должно)
        }

        public bool IsUniqeAsset(string name)
        {
            if (GetAssetByName(name) == null) // если имя равно имя - фолс
            {
                return true;
            }
            return false;
        }

        public void AddLoan (Loan nLoan)
        {
            Loans.Add(nLoan);
        }
    }
}
