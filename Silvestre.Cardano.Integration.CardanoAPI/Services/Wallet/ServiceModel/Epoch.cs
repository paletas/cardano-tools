using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI.ServiceModel
{
    internal class Epoch
    {
        [JsonPropertyName("epoch_number")]
        public int Number { get; set; }

        [JsonPropertyName("epoch_start_time")]
        public DateTime StartTime { get; set; }
    }
}
