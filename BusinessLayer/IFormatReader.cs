using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentTransactionProcessService.Models;

namespace PaymentTransactionProcessService.BusinessLayer
{
    public interface IFormatReader
    {
        public IEnumerable<List<string>> Read(string path);
    }
}
