using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI.ServiceModel.StakePools
{
    internal class ListStakePoolsResponse
    {
        [JsonPropertyName("stakePools")]
        public IEnumerable<StakePool> StakePools { get; set; }
    }
}
