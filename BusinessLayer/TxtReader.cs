using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PaymentTransactionProcessService.Models;

namespace PaymentTransactionProcessService.BusinessLayer
{
    public class TxtReader : IFormatReader
    {
        public IEnumerable<List<string>> Read(string path)
        {
            foreach (string line in File.ReadLines(path))
            {
                var parameters = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").ToList();
                yield return parameters;
            }
        }

    }
}
