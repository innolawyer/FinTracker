using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public sealed class Storage
    {
        private static Storage _instance;

        public List<Asset> Assets;
        public List<User> Users;
        public List<Transaction> Transactions;
        public List<string> CashbackCategories;

        public Storage()
        {
            Assets = new List<Asset>();
            Users = new List<User>();
            Transactions = new List<Transaction>();
            CashbackCategories = new List<string>();
        }

        public static Storage GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Storage();
            }
            return _instance;
        }
       
    }
}
