using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public sealed class Storage
    {
        private static Storage _storage;

        public List<User> Users;
        public Asset actualAsset;
        public User actualUser;
        public Transaction actualTransaction;

        Storage()
        {
            Users = new List<User>();
        }

        public static Storage GetStorage()
        {
            if (_storage == null)
            {
                _storage = new Storage();
            }
            return _storage;
        }
       
    }
}
