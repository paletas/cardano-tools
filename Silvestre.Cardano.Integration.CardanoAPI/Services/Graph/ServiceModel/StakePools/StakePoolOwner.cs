using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI.ServiceModel.StakePools
{
    internal class StakePoolOwner
    {
        [JsonPropertyName("hash")]
        public string Address { get; set; }
    }
}
