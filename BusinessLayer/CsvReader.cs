using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.BusinessLayer
{
    public class CsvReader : IFormatReader
    {
        public IEnumerable<List<string>> Read(string path)
        {
            foreach (string line in File.ReadLines(path).Skip(1))
            {
                var parameters = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").ToList();
                yield return parameters;
            }
        }
    }
}
