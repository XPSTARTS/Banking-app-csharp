using BankingApp.Interfaces;
using BankingApp.Services;
using System;

namespace BankingApp
{
    internal class Program
    {
        private static readonly IAccountService _accountService = new AccountService();

        static void Main(string[] args)
        {
            Console.Title = "Banking Application";

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("     BANKING APPLICATION     ");
                Console.WriteLine("=========================================\n");

                Console.WriteLine("1. Create New Account");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Transfer Money");
                Console.WriteLine("5. Check Balance");
                Console.WriteLine("6. View Account Details");
                Console.WriteLine("7. View Transaction History");
                Console.WriteLine("8. Admin Panel");
                Console.WriteLine("9. Exit");
                Console.Write("\nEnter your choice (1-9): ");

                string? choice = Console.ReadLine();
                if (choice == null) continue; 

                try
                {
                    switch (choice)
                    {
                        case "1": _accountService.CreateAccount(); break;
                        case "2": _accountService.Deposit(); break;
                        case "3": _accountService.Withdraw(); break;
                        case "4": _accountService.Transfer(); break;
                        case "5": _accountService.CheckBalance(); break;
                        case "6": _accountService.ViewAccountDetails(); break;
                        case "7": _accountService.ViewTransactionHistory(); break;
                        case "8": AdminMenu(); break;
                        case "9":
                            Console.WriteLine("\nThank you for using BankingApp. Goodbye! 👋");
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        // Admin Menu
        private static void AdminMenu()
        {
            if (!AdminLogin())
                return;

            bool adminRunning = true;
            while (adminRunning)
            {
                Console.Clear();
                Console.WriteLine("=========================================");
                Console.WriteLine("           ADMIN PANEL           ");
                Console.WriteLine("=========================================\n");

                Console.WriteLine("1. View All Accounts");
                Console.WriteLine("2. Freeze Account");
                Console.WriteLine("3. Unfreeze Account");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("\nEnter choice (1-4): ");

                string? choice = Console.ReadLine();
                if (choice == null) continue;

                switch (choice)
                {
                    case "1":
                        _accountService.ViewAllAccounts();
                        break;
                    case "2":
                        _accountService.FreezeAccount();
                        break;
                    case "3":
                        _accountService.UnfreezeAccount();
                        break;
                    case "4":
                        adminRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }

                if (adminRunning)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static bool AdminLogin()
        {
            Console.Clear();
            Console.WriteLine("=== ADMIN LOGIN ===\n");

            Console.Write("Username: ");
            string username = Console.ReadLine()?.Trim();

            Console.Write("Password: ");
            string password = Console.ReadLine()?.Trim();

            if (username?.ToLower() == "admin" && password == "admin1234")
            {
                Console.WriteLine("\nLogin successful! Welcome Admin.");
                return true;
            }
            else
            {
                Console.WriteLine("\nInvalid admin credentials.");
                return false;
            }
        }
    }
}