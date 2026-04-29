using BankingApp.Interfaces;
using BankingApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BankingApp.Services
{
    internal class FileService : IFileService
    {
        private readonly string _filePath = "BankingData.json";

        public void SaveAccounts(List<BankAccount> accounts)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve   
                };

                string json = JsonSerializer.Serialize(accounts, options);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public List<BankAccount> LoadAccounts()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<BankAccount>();

                string json = File.ReadAllText(_filePath);

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                return JsonSerializer.Deserialize<List<BankAccount>>(json, options) ?? new List<BankAccount>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<BankAccount>();
            }
        }
    }
}