using System;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    internal class Block
    {
        public long Id { get; set; }

        public byte[] Hash { get; set; }

        public uint EpochNumber { get; set; }

        public uint SlotNumber { get; set; }

        public uint EpochSlotNumber { get; set; }

        public uint BlockNumber { get; set; }

        public long PreviousID { get; set; }

        public long SlotLeaderId { get; set; }

        public uint Size { get; set; }

        public DateTime Timestamp { get; set; }

        public ulong TransactionCount { get; set; }
    }
}
