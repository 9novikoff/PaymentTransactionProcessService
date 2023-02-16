using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.Models
{
    public class Service
    {
        public string Name { get; set; }
        public List<Payer> Payers = new List<Payer>();
        public decimal Total { get; set; }
    }
}
