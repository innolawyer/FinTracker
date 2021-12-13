using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker
{
    public class Storage
    {
        private static Storage _storage;

        public List<User> Users;
        public Asset actualAsset;
        public User actualUser;
        public Transaction actualTransaction;
        public Loan actualLoan;

        [Flags]
        public enum sign
        {
            spend,
            income
        }

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

        public User GetUserByName(string name)
        {
            foreach (User user in Users)
            {
                if (user.Name == name)
                {
                    return user;
                }
            }
            //throw new Exception();
            return null;
        }

        public Loan GetLoanById(int id)
        {
            Storage storage = Storage.GetStorage();
            foreach (Loan loan in storage.actualUser.Loans)
            {
                if (loan.Id == id)
                {
                    return loan;
                }
            }
            return null;
        }


        public void DeleteUser(string name)
        {
            User user = GetUserByName(name);
            Users.Remove(user);
        }

        public bool IsUniqeUser(string name) // узнать куда это деть (как зашить в конструктор??)
        {
            bool uniq = true;
            foreach (User user in Users)
            {
                if (name == user.Name)
                {
                    uniq = false;
                }
            }
            return uniq;
        }
    }
}
