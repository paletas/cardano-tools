using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.StakePools
{
    public class ListStakePoolsResponse
    {
        [JsonPropertyName("total")]
        public ulong Total { get; set; }

        [JsonPropertyName("from")]
        public ulong From { get; set; }

        [JsonPropertyName("stakePools")]
        public IEnumerable<StakePool> StakePools { get; set; }
    }
}
