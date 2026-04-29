using System;

namespace BankingApp.Models
{
    internal class Transaction
    {
        public int TransactionId { get; set; }
        public string AccountNumber { get; set; }
        public string Type { get; set; }           
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }      

        public Transaction(string accountNumber, string type, double amount, string description = "", string reference = "")
        {
            TransactionId = new Random().Next(100000, 999999);
            AccountNumber = accountNumber;
            Type = type;
            Amount = amount;
            Date = DateTime.Now;
            Description = description;
            Reference = reference;
        }

        public override string ToString()
        {
            string refInfo = string.IsNullOrEmpty(Reference) ? "" : $" → {Reference}";
            return $"[{Date:dd-MM-yyyy HH:mm}] | {Type,-18} | Rs. {Amount,10:F2} | {Description}{refInfo}";
        }
    }
}