using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentTransactionProcessService.Models;

namespace PaymentTransactionProcessService.BusinessLayer
{
    public class TxtReader : IFormatReader
    {
        public IEnumerable<List<string>> Read(string path)
        {
            //TO LINQ!
            foreach (string line in File.ReadLines(path))
            {
                var parameters = line.Split(',').ToList();
                yield return parameters;
            }
        }

    }
}
