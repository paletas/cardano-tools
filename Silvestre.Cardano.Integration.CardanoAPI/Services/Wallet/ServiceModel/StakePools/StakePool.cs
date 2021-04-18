using Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI.ServiceModel.Converters;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI.ServiceModel.StakePools.Model
{
    internal class StakePool
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("metrics")]
        public StakePoolMetrics Metrics { get; set; }

        [JsonPropertyName("cost")]
        public Asset Cost { get; set; }

        [JsonPropertyName("margin")]
        public Asset Margin { get; set; }

        [JsonPropertyName("pledge")]
        public Asset Pledge { get; set; }

        [JsonPropertyName("metadata")]
        public StakePoolMetadata Metadata { get; set; }

        [JsonPropertyName("retirement")]
        public Epoch? Retirement { get; set; }

        [JsonPropertyName("flags")]
        [JsonConverter(typeof(StakePoolFlagsToEnum))]
        public StakePoolsFlagEnum Flags { get; set; }
    }
}
