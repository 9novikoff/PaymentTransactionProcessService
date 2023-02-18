using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.Models
{
    public class Payer
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("payment")]
        public decimal Payment { get; set; }
        [JsonProperty("date")]
        public DateOnly Date { get; set; }
        [JsonProperty("account_number")]
        public long AccountNumber { get; set; }
    }
}
