using System;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.Epoch
{
    public class CurrentEpoch
    {
        [JsonPropertyName("number")]
        public ulong Number { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("transactionCount")]
        public ulong TransactionCount { get; set; }

        [JsonPropertyName("blockCount")]
        public ulong BlockCount { get; set; }

        [JsonPropertyName("slotNumber")]
        public uint SlotNumber { get; set; }

        [JsonPropertyName("maximumSlots")]
        public uint MaximumSlots { get; set; }
    }
}
