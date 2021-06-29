using System;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.Epoch
{
    public class Epoch
    {
        [JsonPropertyName("number")]
        public ulong Number { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("transactionsCount")]
        public ulong TransactionsCount { get; set; }

        [JsonPropertyName("blockCount")]
        public ulong BlocksCount { get; set; }

        [JsonPropertyName("maximumSlots")]
        public uint MaximumSlots { get; set; }

        [JsonPropertyName("transactionsTotal")]
        public decimal TransactionsTotal { get; set; }

        [JsonPropertyName("feesTotal")]
        public decimal FeesTotal { get; set; }
    }
}
