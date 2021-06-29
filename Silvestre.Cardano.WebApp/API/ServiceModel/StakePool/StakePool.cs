using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.StakePool
{
    [DebuggerDisplay("{PoolAddress}")]
    public class StakePool
    {
        [JsonPropertyName("poolAddress")]
        public string PoolAddress { get; internal set; }

        [JsonPropertyName("ticker")]
        public string Ticker { get; internal set; }

        [JsonPropertyName("name")]
        public string Name { get; internal set; }

        [JsonPropertyName("description")]
        public string Description { get; internal set; }

        [JsonPropertyName("websiteUrl")]
        public Uri Website { get; internal set; }

        [JsonPropertyName("maintenanceCostInADA")]
        public decimal MaintenanceInADA { get; internal set; }

        [JsonPropertyName("marginPercentage")]
        public decimal MarginPercentage { get; internal set; }

        [JsonPropertyName("pledgeInADA")]
        public decimal PledgeInADA { get; internal set; }

        [JsonPropertyName("delegatedInADA")]
        public decimal DelegationInADA { get; internal set; }

        [JsonPropertyName("metadataUrl")]
        public Uri MetadataUrl { get; internal set; }
    }
}
