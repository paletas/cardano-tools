using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI.ServiceModel
{
    internal class Asset
    {
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }
    }
}
