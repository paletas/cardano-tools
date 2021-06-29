using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.Epoch
{
    public class EpochStatistics
    {
        [JsonPropertyName("epochNumber")]
        public ulong EpochNumber { get; set; }

        [JsonPropertyName("totalStakePools")]
        public ulong TotalStakePools { get; set; }

        [JsonPropertyName("totalDelegations")]
        public ulong TotalDelegations { get; set; }

        [JsonPropertyName("circulatingSupply")]
        public decimal CirculatingSupply { get; set; }

        [JsonPropertyName("stakedSupply")]
        public decimal StakedSupply { get; set; }

        [JsonPropertyName("rewards")]
        public decimal? Rewards { get; set; }

        [JsonPropertyName("orphanedRewards")]
        public decimal? OrphanedRewards { get; set; }
    }
}
