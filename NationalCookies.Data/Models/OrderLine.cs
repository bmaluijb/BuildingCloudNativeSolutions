using Newtonsoft.Json;
using System;

namespace NationalCookies.Data
{
    public class OrderLine
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public Cookie Cookie { get; set; }
        public Order Order { get; set; }
    }
}
