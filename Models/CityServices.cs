using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.Models
{
    public class CityServices
    {
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("services")]
        public List<Service> Services = new List<Service>();
        [JsonProperty("total")]
        public decimal Total { get; set; }
    }
}
