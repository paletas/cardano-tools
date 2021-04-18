using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI.ServiceModel.StakePools
{
    internal class StakePoolMetrics
    {
        [JsonPropertyName("non_myopic_member_rewards")]
        public Asset NonMyopicMemberRewards { get; set; }

        [JsonPropertyName("relative_stake")]
        public Asset RelativeStake { get; set; }

        [JsonPropertyName("saturation")]
        public decimal Saturation { get; set; }

        [JsonPropertyName("produced_blocks")]
        public Asset ProducedBlocks { get; set; }
    }
}
