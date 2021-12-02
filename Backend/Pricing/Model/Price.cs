﻿using Newtonsoft.Json;
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
    }

    public class Price : IDataObject
    {
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; }

        [JsonPropertyName("pack")]
        public Pack Pack { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }
    }
}
