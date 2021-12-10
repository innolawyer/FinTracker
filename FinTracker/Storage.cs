﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using System.Text.Json;
using Newtonsoft.Json;

namespace FinTracker
{
    public class Storage
    {
        private static Storage _storage;

        public List<User> Users;
        public Asset actualAsset;
        public User actualUser;
        public Transaction actualTransaction;

        [Flags]
        public enum sign
        {
            spend,
            income
        }

        private string path = @"C:\Users\azaro\Desktop\QQQ.txt";

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

        public void Save()
        {
            //StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
            string str = JsonConvert.SerializeObject(Users, Formatting.Indented);
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
            {
                sw.WriteLine(str);
                sw.Close();
            }
        }

        public void GetSave()
        {
            string result = "";
            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (FileNotFoundException)
            {

            }
            List<User> ?newClients = JsonConvert.DeserializeObject<List<User>>(result);
            if (newClients is not null)
            {
                Users = newClients;
            }
        }
    }
}
