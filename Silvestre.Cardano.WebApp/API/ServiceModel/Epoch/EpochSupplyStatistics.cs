using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.Epoch
{
    public class EpochSupplyStatistics
    {
        [JsonPropertyName("epochNumber")]
        public ulong EpochNumber { get; set; }

        [JsonPropertyName("circulatingSupply")]
        public decimal CirculatingSupply { get; set; }

        [JsonPropertyName("stakedSupply")]
        public decimal StakedSupply { get; set; }
    }
}
