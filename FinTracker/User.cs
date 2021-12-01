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
            Categories = new List<string>() { "Продукты", "Транспорт", "Здоровье"};
        }

        public void SetCategory(string name)
        {

        }

        public void DropCategory(string name)
        {

        }

        public void DropCategore(string name)
        {

        }

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
