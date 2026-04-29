using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BankingApp.Models
{
    internal class BankAccount
    {
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public Customer Customer { get; set; }
        public bool IsFrozen { get; set; } = false;          

        [JsonInclude]
        public List<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public BankAccount() { }

        public BankAccount(string accountNumber, double balance, Customer customer)
        {
            AccountNumber = accountNumber;
            Balance = balance;
            Customer = customer;
        }

        public void Deposit(double amount, string description = "Cash Deposit")
        {
            if (IsFrozen)
            {
                Console.WriteLine("This account is frozen. Operation not allowed.");
                return;
            }
            if (amount > 0)
            {
                Balance += amount;
                Transactions.Add(new Transaction(AccountNumber, "Deposit", amount, description));
            }
        }

        public bool Withdraw(double amount, string description = "Cash Withdrawal")
        {
            if (IsFrozen)
            {
                Console.WriteLine("This account is frozen. Operation not allowed.");
                return false;
            }
            if (amount > 0 && Balance >= amount)
            {
                Balance -= amount;
                Transactions.Add(new Transaction(AccountNumber, "Withdraw", amount, description));
                return true;
            }
            return false;
        }

        public bool TransferTo(BankAccount destination, double amount)
        {
            if (IsFrozen || destination.IsFrozen)
            {
                Console.WriteLine("One or both accounts are frozen. Transfer not allowed.");
                return false;
            }
            if (amount > 0 && Balance >= amount)
            {
                Balance -= amount;
                Transactions.Add(new Transaction(AccountNumber, "Transfer-Sent", amount,
                    $"Transferred to {destination.AccountNumber}", destination.AccountNumber));

                destination.Balance += amount;
                destination.Transactions.Add(new Transaction(destination.AccountNumber, "Transfer-Received", amount,
                    $"Received from {AccountNumber}", AccountNumber));

                return true;
            }
            return false;
        }

        public override string ToString()
        {
            string status = IsFrozen ? " [FROZEN]" : "";
            return $"Account: {AccountNumber} | Balance: Rs. {Balance:F2} | Owner: {Customer?.Name}{status}";
        }
    }
}