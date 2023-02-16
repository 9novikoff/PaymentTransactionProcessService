using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PaymentTransactionProcessService.Models;

namespace PaymentTransactionProcessService.BusinessLayer
{
    public class TransactionService
    {
        private readonly IFormatReader _reader;

        TransactionService(IFormatReader reader)
        {
            _reader = reader;
        }

        public void HandleTransaction(string sourcePath, ErrorRepository errorRepository)
        {
            var parameters = _reader.Read(sourcePath);
            var transactions = GetTransactions(parameters, errorRepository);
            var transformedTransactions = TransformTransactions(transactions);

            errorRepository.AddParsedFile();
            var resPath = ConfigurationManager.AppSettings["TransactionsResult"] + @"\" + DateTime.Now.Date + @"\" 
                          + "output" + errorRepository.GetParsedNumber() + ".json";
        }

        private List<CityServices> TransformTransactions(IEnumerable<Transaction> transactions)
        {
            var citiesServices =
            (
                from t1 in transactions
                let cityName = t1.Address.Split(",")[0]
                select new CityServices()
                {
                    City = cityName,
                    Services = (
                        from t2 in transactions
                        where cityName == t2.Address.Split(",")[0]
                        let serviceName = t2.Service
                        select new Service()
                        {
                            Name = serviceName,
                            Payers = (
                                from t3 in transactions
                                where serviceName == t2.Service
                                select new Payer()
                                {
                                    Name = t3.FirstName + " " + t3.LastName,
                                    Payment = t3.Payment,
                                    Date = t3.Date,
                                    AccountNumber = t3.AccountNumber
                                }
                            ).ToList()
                        }
                    ).ToList()
                }).ToList();

            foreach (var cityServices in citiesServices)
            {
                foreach (var service in cityServices.Services)
                {
                    service.Total = service.Payers.Sum(p => p.Payment);
                }

                cityServices.Total = cityServices.Services.Sum(s => s.Total);
            }
            return citiesServices;
        }

        private IEnumerable<Transaction> GetTransactions(IEnumerable<List<string>> parameters, ErrorRepository errorRepository)
        {
            //TO LINQ!
            foreach (var item in parameters)
            {
                var transaction = GetValidatedTransaction(item);
                if (transaction != null)
                {
                    errorRepository.AddParsedLine();
                    yield return transaction;
                }

                errorRepository.AddError();
            }
        }

        private Transaction? GetValidatedTransaction(List<string> parameters)
        {
            if (parameters.Count != typeof(Transaction).GetProperties().Length)
            {
                return null;
            }

            int propertyIndex = 0;
            var firstName = parameters[propertyIndex];
            var lastName = parameters[propertyIndex++];
            var address = parameters[propertyIndex++];
            var canBeParsedToDecimal = decimal.TryParse(parameters[propertyIndex++],out var payment);
            var canBeParsedToDate = DateTime.TryParse(parameters[propertyIndex++], out var date);
            var canBeParsedToLong = long.TryParse(parameters[propertyIndex++], out var accountNumber);
            var service = parameters[propertyIndex++];

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(service))
                return null;

            if(!canBeParsedToDate || !canBeParsedToDecimal || !canBeParsedToLong)
                return null;

            return new Transaction(firstName, lastName, address, payment, date, accountNumber, service);
        }

        private async Task SaveTransactionsAsync(string path, List<CityServices> citiesServices)
        {
            var jsonString = JsonSerializer.Serialize(citiesServices);

            await File.WriteAllTextAsync(path, jsonString);
        }
    }
}
