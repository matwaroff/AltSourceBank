using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AltSourceApp.Models
{

    public class Account
    {
        public string id { get; }
        public DateTime? creationDate;
        public string passwordHash { get; set; }
        public string username { get; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public decimal balance { get; set; }
        public string authToken { get; set; }
        public List<Transaction> transactionHistory { get; set; }

        private const string userFile = @"userlogins.json";
        private const int historySize = 30;

        public Account(string username, string passwordHash, string firstName = "", string lastName = "")
        {
            this.id = Guid.NewGuid().ToString();
            this.firstName = firstName;
            this.lastName = lastName;
            this.username = username;
            this.passwordHash = passwordHash;
            this.transactionHistory = new List<Transaction>();
        }

        public static Account Login(string username, string password)
        {
            //Check DB in production
            JArray accounts = JArray.Parse(File.ReadAllText(userFile));

            foreach(JToken token in accounts)
            {
                Account acct = token.ToObject<Account>(); 
                if(acct.username == username)
                {
                    string hashString = HashPassword(password);
                    if (acct.passwordHash == hashString)
                    {
                        string newAuthToken = Guid.NewGuid().ToString();
                        token["authToken"] = newAuthToken;
                        acct.authToken = newAuthToken;

                        File.WriteAllLines(userFile, ConvertToStringArray(accounts.ToString()));
                        return acct;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public bool Save()
        {
            JArray accounts = JArray.Parse(File.ReadAllText(userFile));

            //Check for existing account
            bool existingAcct = accounts.Select(acct => (string)acct["username"] ).Where(username => username == this.username).FirstOrDefault() != null;
            

            if (!existingAcct)
            {
                this.creationDate = DateTime.Now;
                JObject newAcct = JObject.FromObject(this);
                accounts.Add(newAcct);
                File.WriteAllLines(userFile, ConvertToStringArray(accounts.ToString()));
                return true;
            }
            return false;
        }

        public bool Deposit(decimal amount)
        {
            JArray accounts = JArray.Parse(File.ReadAllText(userFile));

            JToken acct = accounts.Where(x => (string)x["username"] == this.username).FirstOrDefault();
            balance += amount;
            acct["balance"] = balance;

            transactionHistory.Insert(0, new Transaction(amount));
            acct["transactionHistory"] = JArray.FromObject(transactionHistory);

            File.WriteAllLines(userFile, ConvertToStringArray(accounts.ToString()));

            return true;
        }

        public bool Withdrawl(decimal amount)
        {
            if(balance - amount < 0) return false;

            JArray accounts = JArray.Parse(File.ReadAllText(userFile));
            JToken acct = accounts.Where(x => (string)x["username"] == this.username).FirstOrDefault();
            balance -= Math.Abs(amount);
            acct["balance"] = balance;

            transactionHistory.Insert(0, new Transaction(-amount));
            acct["transactionHistory"] = JArray.FromObject(transactionHistory);

            File.WriteAllLines(userFile, ConvertToStringArray(accounts.ToString()));
            return true;
        }

        public static string HashPassword(string password)
        {
            StringBuilder result = new StringBuilder();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < hashArray.Length; i++)
                {
                    result.Append(hashArray[i].ToString("X2"));
                }
            }
            return result.ToString();
        }

        public static Account FromAuthToken(string authToken)
        {
            JArray accounts = JArray.Parse(File.ReadAllText(userFile));
            JToken authenticatedUser = accounts.Where(acct => (string)acct["authToken"] == authToken).FirstOrDefault();
            return authenticatedUser != null
                ? authenticatedUser.ToObject<Account>()
                : null;
        }

        private static string[] ConvertToStringArray(string source)
        {
            return source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }
        private JToken trimHistory(JToken account)
        {
            if(transactionHistory.Count > historySize)
            {
                transactionHistory.RemoveAt(historySize);
                var tokenHistory = account["transactionHistory"].ToObject<List<Transaction>>();
                tokenHistory.RemoveAt(historySize);
                account["transactionHistory"] = JArray.FromObject(tokenHistory);
            }
            return account;
        }
    }
}
