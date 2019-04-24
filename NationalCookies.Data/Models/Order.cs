using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NationalCookies.Data
{
    public class Order
    {
        public Order()
        {

            OrderLines = new List<OrderLine>();
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public double Price { get; set; }

        public string Status { get; set; }

        public List<OrderLine> OrderLines { get; set; }



    }
}
