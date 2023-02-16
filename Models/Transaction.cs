using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.Models
{
    public class Transaction
    {
        public Transaction(string firstName, string lastName, string address, decimal payment, DateTime date, long accountNumber, string service)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            Payment = payment;
            Date = date;
            AccountNumber = accountNumber;
            Service = service;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long AccountNumber { get; set; }
        public string Service { get; set; }
    }
}
