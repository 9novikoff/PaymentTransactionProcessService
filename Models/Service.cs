using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.Models
{
    public class Service
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("payers")]
        public List<Payer> Payers = new List<Payer>();
        [JsonProperty("total")]
        public decimal Total { get; set; }
    }
}
