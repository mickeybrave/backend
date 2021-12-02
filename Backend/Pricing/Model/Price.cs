using System.Text.Json.Serialization;

namespace Backend.Model
{
    public interface IDataObject
    {
        string ProductCode { get; set; }
    }

    public class Pack
    {
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
    public class Price : IDataObject
    {
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; }

        [JsonPropertyName("eachPrice")]
        public double EachItemPrice { get; set; }

        [JsonPropertyName("pack")]
        public Pack Pack { get; set; }

    }
}
