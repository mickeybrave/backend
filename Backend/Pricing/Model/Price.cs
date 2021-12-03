using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Backend.Model
{

    public interface IDataObject
    {
        string ProductCode { get; set; }
    }

    public class Pack
    {
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }

        public bool Exists { get { return Count != 0 && Amount != 0; } }
    }

    public class Price : IDataObject
    {
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; }
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("pack")]
        public Pack Pack { get; set; }


    }
}
