using Newtonsoft.Json;
using System;

namespace AltSourceApp.Models
{
    public class Transaction
    {
        public decimal amount { get; set; }
        public DateTime dateTime { get; set; }

        
        

        public Transaction(decimal amount)
        {
            this.amount = amount;
            dateTime = DateTime.Now;
        }

        [JsonConstructor]
        public Transaction(decimal amount, DateTime dateTime)
        {
            this.amount = amount;
            this.dateTime = dateTime;
        }
    }
}