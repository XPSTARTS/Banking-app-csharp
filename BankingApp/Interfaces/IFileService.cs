using BankingApp.Models;
using System.Collections.Generic;

namespace BankingApp.Interfaces
{
    internal interface IFileService
    {
        void SaveAccounts(List<BankAccount> accounts);
        List<BankAccount> LoadAccounts();
    }
}