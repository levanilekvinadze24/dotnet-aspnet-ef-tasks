using System.Text.Json.Serialization;

namespace CountryServices
{
    internal sealed class LocalCurrencyInfo
    {
        [JsonPropertyName("name")]
        public string? CountryName { get; set; }

        [JsonPropertyName("currencies")]
        public Currency[]? Currencies { get; set; }

        public sealed class Currency
        {
            [JsonPropertyName("code")]
            public string? Code { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("symbol")]
            public string? Symbol { get; set; }
        }
    }
}
