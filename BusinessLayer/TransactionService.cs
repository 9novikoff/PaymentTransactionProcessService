using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentTransactionProcessService.Models;

namespace PaymentTransactionProcessService.BusinessLayer
{
    public class TransactionService
    {
        private readonly IFormatReader _reader;

        public TransactionService(IFormatReader reader)
        {
            _reader = reader;
        }

        public void HandleTransaction(string sourcePath, ErrorRepository errorRepository)
        {
            var parameters = _reader.Read(sourcePath);
            var transactions = GetTransactions(parameters, errorRepository);
            var transformedTransactions = TransformTransactions(transactions);

            errorRepository.AddParsedFile();

            var directory = ConfigurationManager.AppSettings["TransactionsResult"] + @"\" +
                            DateTime.Now.ToString("MM-dd-yyyy");
            Directory.CreateDirectory(directory);

            var resPath = directory + @"\output" + errorRepository.GetParsedNumber() + ".json";

            SaveTransactionsAsync(resPath, transformedTransactions);
        }

        private List<CityServices> TransformTransactions(IEnumerable<Transaction> transactions)
        {
            var citiesServices =
            (
                from t1 in transactions
                let cityName = t1.Address.Split(";")[0]
                select new CityServices()
                {
                    City = cityName,
                    Services = (
                        from t2 in transactions
                        where cityName == t2.Address.Split(";")[0]
                        let serviceName = t2.Service
                        select new Service()
                        {
                            Name = serviceName,
                            Payers = (
                                from t3 in transactions
                                where serviceName == t3.Service && cityName == t3.Address.Split(";")[0]
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
                }).GroupBy(c => c.City).Select(s => s.First()).ToList();

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

            parameters = parameters.Select(p => p.Trim()).ToList();

            int propertyIndex = 0;
            var firstName = parameters[propertyIndex++];
            var lastName = parameters[propertyIndex++];
            var address = parameters[propertyIndex++];
            var canBeParsedToDecimal = decimal.TryParse(parameters[propertyIndex++],out var payment);
            var canBeParsedToDate = DateTime.TryParseExact(parameters[propertyIndex++], "yyyy-dd-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
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
            var jsonString = JsonConvert.SerializeObject(citiesServices);

            await File.WriteAllTextAsync(path, jsonString);
        }

    }
}
