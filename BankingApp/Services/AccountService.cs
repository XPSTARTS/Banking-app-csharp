using BankingApp.Interfaces;
using BankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BankingApp.Services
{
    internal class AccountService : IAccountService
    {
        private readonly List<BankAccount> _accounts;
        private readonly IFileService _fileService;

        public AccountService()
        {
            _fileService = new FileService();
            _accounts = _fileService.LoadAccounts();
        }

        public void CreateAccount()
        {
            Console.Clear();
            Console.WriteLine("=== Create New Account ===\n");

            string name = GetValidName();
            string phone = GetValidPhone();
            string cnic = GetValidCNIC();

            int userId = new Random().Next(10000, 99999);
            Customer customer = new Customer(userId, name, phone, cnic);

            string accNo = GenerateAccountNumber(userId);
            BankAccount account = new BankAccount(accNo, 0.0, customer);

            _accounts.Add(account);
            _fileService.SaveAccounts(_accounts);

            Console.WriteLine("\nAccount Created Successfully!");
            Console.WriteLine($"Account Number : {accNo}");
            Console.WriteLine($"Customer       : {name}");
            Console.WriteLine($"CNIC           : {cnic}");
        }

        public void Deposit()
        {
            Console.Clear();
            Console.WriteLine("=== Deposit Money ===\n");
            var account = FindAccount();
            if (account == null) return;

            Console.Write("Enter amount to deposit: Rs. ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                account.Deposit(amount);
                _fileService.SaveAccounts(_accounts);
                Console.WriteLine($"Rs. {amount:F2} deposited successfully!");
                Console.WriteLine($"New Balance: Rs. {account.Balance:F2}");
            }
            else
                Console.WriteLine("Invalid amount.");
        }

        public void Withdraw()
        {
            Console.Clear();
            Console.WriteLine("=== Withdraw Money ===\n");
            var account = FindAccount();
            if (account == null) return;

            Console.Write("Enter amount to withdraw: Rs. ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                if (account.Withdraw(amount))
                {
                    _fileService.SaveAccounts(_accounts);
                    Console.WriteLine($"Rs. {amount:F2} withdrawn successfully. Remaining: Rs. {account.Balance:F2}");
                }
                else
                    Console.WriteLine("Insufficient balance!");
            }
            else
                Console.WriteLine("Invalid amount.");
        }

        public void Transfer()
        {
            Console.Clear();
            Console.WriteLine("=== Transfer Money ===\n");

            Console.Write("Enter Source Account Number: ");
            string? sourceAcc = Console.ReadLine()?.Trim();
            var source = _accounts.Find(a => a.AccountNumber == sourceAcc);

            if (source == null)
            {
                Console.WriteLine("Source account not found.");
                return;
            }

            Console.Write("Enter Destination Account Number: ");
            string? destAcc = Console.ReadLine()?.Trim();
            var destination = _accounts.Find(a => a.AccountNumber == destAcc);

            if (destination == null)
            {
                Console.WriteLine("Destination account not found.");
                return;
            }

            if (source == destination)
            {
                Console.WriteLine("Cannot transfer to the same account.");
                return;
            }

            Console.Write("Enter amount to transfer: Rs. ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                if (source.TransferTo(destination, amount))
                {
                    _fileService.SaveAccounts(_accounts);
                    Console.WriteLine($"Transfer of Rs. {amount:F2} successful!");
                }
                else
                    Console.WriteLine("Insufficient balance in source account.");
            }
            else
                Console.WriteLine("Invalid amount.");
        }

        public void CheckBalance()
        {
            Console.Clear();
            Console.WriteLine("=== Check Balance ===\n");
            var account = FindAccount();
            if (account != null)
                Console.WriteLine($"Current Balance: Rs. {account.Balance:F2}");
            else
                Console.WriteLine("No Account Found!");
        }

        public void ViewAccountDetails()
        {
            Console.Clear();
            Console.WriteLine("=== Account Details ===\n");
            var account = FindAccount();
            if (account != null)
                Console.WriteLine(account);
            else
                Console.WriteLine("No Account Found!");
        }

        public void ViewTransactionHistory()
        {
            Console.Clear();
            Console.WriteLine("=== Transaction History ===\n");

            var account = FindAccount();
            if (account == null) return;

            Console.WriteLine($"Account: {account.AccountNumber} | Customer: {account.Customer.Name}");
            Console.WriteLine($"Current Balance: Rs. {account.Balance:F2}\n");
            Console.WriteLine("──────────────────────────────────────────────────────────────");

            if (account.Transactions.Count == 0)
            {
                Console.WriteLine("No transactions yet.");
                return;
            }

            foreach (var txn in account.Transactions.OrderByDescending(t => t.Date))
            {
                Console.WriteLine(txn);
            }
        }

        // ==================== ADMIN METHODS ====================

        public List<BankAccount> GetAllAccounts() => _accounts;

        public void ViewAllAccounts()
        {
            Console.Clear();
            Console.WriteLine("=== ALL ACCOUNTS ===\n");

            if (_accounts.Count == 0)
            {
                Console.WriteLine("No accounts found.");
                return;
            }

            foreach (var acc in _accounts)
            {
                Console.WriteLine(acc);
            }
            Console.WriteLine($"\nTotal Accounts: {_accounts.Count}");
        }

        public void FreezeAccount()
        {
            Console.Clear();
            Console.WriteLine("=== Freeze Account ===\n");

            var account = FindAccount();
            if (account == null) return;

            if (account.IsFrozen)
                Console.WriteLine("Account is already frozen.");
            else
            {
                account.IsFrozen = true;
                _fileService.SaveAccounts(_accounts);
                Console.WriteLine($"Account {account.AccountNumber} has been FROZEN.");
            }
        }

        public void UnfreezeAccount()
        {
            Console.Clear();
            Console.WriteLine("=== Unfreeze Account ===\n");

            var account = FindAccount();
            if (account == null) return;

            if (!account.IsFrozen)
                Console.WriteLine("Account is already active.");
            else
            {
                account.IsFrozen = false;
                _fileService.SaveAccounts(_accounts);
                Console.WriteLine($"Account {account.AccountNumber} has been UNFROZEN.");
            }
        }

        // ==================== HELPER METHODS ============

        private BankAccount FindAccount()
        {
            Console.Write("Enter Account Number: ");
            string? accNo = Console.ReadLine()?.Trim();
            var account = _accounts.Find(a => a.AccountNumber == accNo);
            if (account == null)
                Console.WriteLine("Account not found!");
            return account;
        }

        private string GetValidName()
        {
            while (true)
            {
                Console.Write("Enter Full Name: ");
                string? name = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(name) && name.Length >= 3)
                    return name;
                Console.WriteLine("Name must be at least 3 characters.");
            }
        }

        private string GetValidPhone()
        {
            while (true)
            {
                Console.Write("Enter Phone (03XX-XXXXXXX): ");
                string? phone = Console.ReadLine()?.Trim();
                if (Regex.IsMatch(phone, @"^03[0-9]{2}-?[0-9]{7}$"))
                    return phone.Replace("-", "");
                Console.WriteLine("Invalid phone format. Use 03XX-XXXXXXX");
            }
        }

        private string GetValidCNIC()
        {
            while (true)
            {
                Console.Write("Enter CNIC (XXXXX-XXXXXXX-X): ");
                string? cnic = Console.ReadLine()?.Trim();
                if (Regex.IsMatch(cnic, @"^\d{5}-\d{7}-\d$"))
                    return cnic;
                Console.WriteLine("Invalid CNIC. Use format: 12345-1234567-1");
            }
        }

        private string GenerateAccountNumber(int userId)
        {
            return $"BA-{DateTime.Now:yyyyMMdd}-{userId:D5}";
        }
    }
}