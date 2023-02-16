using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.BusinessLayer
{
    internal class CsvReader : IFormatReader
    {
        public IEnumerable<List<string>> Read(string path)
        {
            //TO sLINQ!
            foreach (string line in File.ReadLines(path).Skip(1))
            {
                var parameters = line.Split(',').ToList();
                yield return parameters;
            }
        }
    }
}
