using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.SignalR.ServiceModel
{
    public class Block
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("epoch")]
        public uint EpochNumber { get; set; }

        [JsonPropertyName("slotNumber")]
        public uint SlotNumber { get; set; }

        [JsonPropertyName("epochSlotNumber")]
        public uint EpochSlotNumber { get; set; }

        [JsonPropertyName("blockNumber")]
        public uint BlockNumber { get; set; }

        [JsonPropertyName("size")]
        public uint Size { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
