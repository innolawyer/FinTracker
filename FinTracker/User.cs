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
        public List<string> Categories;
        public List<Asset> Assets = new List<Asset>();

        public User(string name) // проверка на уникальность (?)
        {
            Name = name;
            Categories = new List<string>() {"Супермаркет", "Транспорт", "Коммунальные платежи", "Снятия",
            "Одежда и обувь", "Рестораны", "Отдых и развлечения", "Здоровье и красота", "Комиссия", "Онлайн сервисы и подписки",
            "Связь и интернет", "Дом и ремонт", "Животные", "Прочие расходы", "Подарки"};
        }

        //public void SetCategory(string name)
        //{
        //    Categories.Add(name);
        //}

        //public void DropCategory(string name)
        //{
        //    Categories.Remove(name);
        //}

        //public void DropAllCategories()
        //{
        //    Categories.Clear();
        //}

        public void AddAsset(string name, double startAmount, double interest, double cashback, double fee) // Добавляет счета
        {
            Assets.Add(new Asset(name, startAmount)); // это бумажные деньги. Надом исправить как таока научимся делать другие счета
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
    }
}
