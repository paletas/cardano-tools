using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.Transactions
{
    public class Transaction
    {
        [JsonPropertyName("transactionHash")]
        public string TransactionHash { get; set; }

        [JsonPropertyName("epochNumber")]
        public uint EpochNumber { get; set; }

        [JsonPropertyName("slotNumber")]
        public uint SlotNumber { get; set; }

        [JsonPropertyName("epochSlotNumber")]
        public uint EpochSlotNumber { get; set; }

        [JsonPropertyName("blockNumber")]
        public uint BlockNumber { get; set; }

        [JsonPropertyName("transactionCount")]
        public ulong TransactionCount { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("blockSize")]
        public uint BlockSize { get; set; }

        [JsonPropertyName("transactionOutputTotal")]
        public decimal TransactionOutputTotal { get; set; }

        [JsonPropertyName("fees")]
        public decimal Fees { get; set; }

        [JsonPropertyName("deposit")]
        public decimal Deposit { get; set; }

        [JsonPropertyName("transactionSize")]
        public uint TransactionSize { get; set; }

        [JsonPropertyName("invalidBeforeBlock")]
        public ulong? InvalidBeforeBlock { get; set; }

        [JsonPropertyName("invalidAfterBlock")]
        public ulong? InvalidAfterBlock { get; set; }

        [JsonPropertyName("transactionInId")]
        public ulong? TransactionInId { get; set; }

        [JsonPropertyName("output")]
        public IEnumerable<TransactionOutput> Output { get; set; }

        [JsonPropertyName("metadata")]
        public IEnumerable<TransactionMetadata> Metadata { get; set; }
    }

    public class TransactionOutput
    {
        [JsonPropertyName("addressTo")]
        public string AddressTo { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }

    public class TransactionMetadata
    {
        [JsonPropertyName("key")]
        public ulong? Key { get; set; }

        [JsonPropertyName("json")]
        public string? Json { get; set; }
    }
}
