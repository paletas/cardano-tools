using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.Block
{
    public class Block
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("epochNumber")]
        public uint EpochNumber { get; set; }

        [JsonPropertyName("slotNumber")]
        public uint SlotNumber { get; set; }

        [JsonPropertyName("epochSlotNumber")]
        public uint EpochSlotNumber { get; set; }

        [JsonPropertyName("number")]
        public uint BlockNumber { get; set; }

        [JsonPropertyName("previousId")]
        public long PreviousID { get; set; }

        [JsonPropertyName("slotLeader")]
        public long SlotLeader { get; set; }

        [JsonPropertyName("size")]
        public uint Size { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
