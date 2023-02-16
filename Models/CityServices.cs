using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.Models
{
    public class CityServices
    {
        public string City { get; set; }
        public List<Service> Services = new List<Service>();
        public decimal Total { get; set; }
    }
}
