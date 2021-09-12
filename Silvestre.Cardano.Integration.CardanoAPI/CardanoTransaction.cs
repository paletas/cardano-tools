namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoTransaction
    {
        public ulong TransactionId { get; set; }

        public string TransactionHash { get; set; }

        public uint EpochNumber { get; set; }

        public uint SlotNumber { get; set; }

        public uint EpochSlotNumber { get; set; }

        public uint BlockNumber { get; set; }

        public ulong TransactionCount { get; set; }

        public DateTime Timestamp { get; set; }

        public uint BlockSize { get; set; }

        public CardanoAsset TransactionOutputTotal { get; set; }

        public CardanoAsset Fees { get; set; }

        public CardanoAsset Deposit { get; set; }

        public uint TransactionSize { get; set; }

        public ulong? InvalidBeforeBlock { get; set; }

        public ulong? InvalidAfterBlock { get; set; }

        public ulong? TransactionInId { get; set; }

        public IEnumerable<CardanoTransactionOutput> Output { get; set; }

        public IEnumerable<CardanoTransactionMetadata> Metadata { get; set; }
    }

    public class CardanoTransactionOutput
    {
        public string AddressTo { get; set; }

        public CardanoAsset Output { get; set; }
    }

    public class CardanoTransactionMetadata
    {
        public ulong? MetadataKey { get; set; }

        public string? MetadataJson { get; set; }
    }
}
