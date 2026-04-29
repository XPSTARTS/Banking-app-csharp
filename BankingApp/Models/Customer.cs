using System;

namespace BankingApp.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string CNIC { get; set; }

        public Customer(int id, string name, string phoneNo, string cnic)
        {
            Id = id;
            Name = name;
            PhoneNo = phoneNo;
            CNIC = cnic;
        }

        public override string ToString()
        {
            return $"ID: {Id} | Name: {Name} | Phone: {PhoneNo} | CNIC: {CNIC}";
        }
    }
}