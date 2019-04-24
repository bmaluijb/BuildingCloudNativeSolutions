using Newtonsoft.Json;
using System;

namespace NationalCookies.Data
{
    public class Cookie
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public double Price { get; set; }
    }
}
