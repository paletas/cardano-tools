namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    public class Transaction
    {
        public ulong TransactionId { get; set; }

        public string TransactionHash { get; set; }

        public uint EpochNumber { get; set; }

        public uint SlotNumber { get; set; }

        public uint EpochSlotNumber { get; set; }

        public uint BlockNumber { get; set; }

        public ulong TransactionCount { get; set; }

        public DateTime BlockTimestamp { get; set; }

        public uint BlockSize { get; set; }

        public ulong TransactionOutputTotal { get; set; }

        public ulong Fees { get; set; }

        public ulong Deposit { get; set; }

        public uint TransactionSize { get; set; }

        public ulong InvalidBeforeBlock { get; set; }

        public ulong InvalidAfterBlock { get; set; }

        public ulong MetadataKey { get; set; }

        public string MetadataJson { get; set; }

        public ulong TransactionInId { get; set; }
    }
}