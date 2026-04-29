using BankingApp.Models;
using System.Collections.Generic;

namespace BankingApp.Interfaces
{
    internal interface IAccountService
    {
        // Customrt features
        void CreateAccount();
        void Deposit();
        void Withdraw();
        void Transfer();
        void CheckBalance();
        void ViewAccountDetails();
        void ViewTransactionHistory();

        // Admin features
        List<BankAccount> GetAllAccounts();
        void FreezeAccount();
        void UnfreezeAccount();
        void ViewAllAccounts();
    }
}