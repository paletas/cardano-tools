using Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI.ServiceModel.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI.ServiceModel.StakePools
{
    internal class StakePool
    {
        [JsonPropertyName("id")]
        public string PoolAddress { get; set; }

        [JsonPropertyName("rewardAddress")]
        public string RewardAddress { get; set; }

        [JsonPropertyName("url")]
        public Uri MetadataUrl { get; set; }

        [JsonPropertyName("fixedCost")]
        [JsonConverter(typeof(AssetQuantityConverter))]
        public decimal FixedCost { get; set; }

        [JsonPropertyName("pledge")]
        [JsonConverter(typeof(AssetQuantityConverter))]
        public decimal Pledge { get; set; }

        [JsonPropertyName("margin")]
        public decimal MarginPercentage { get; set; }

        [JsonPropertyName("owners")]
        public IEnumerable<StakePoolOwner> Owners { get; set; }
    }
}
