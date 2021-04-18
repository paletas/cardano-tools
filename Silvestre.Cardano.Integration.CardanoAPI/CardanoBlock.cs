using System;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoBlock
    {
        public string Hash { get; set; }

        public uint EpochNumber { get; set; }

        public uint SlotNumber { get; set; }

        public uint EpochSlotNumber { get; set; }

        public uint BlockNumber { get; set; }

        public long PreviousID { get; set; }

        public long SlotLeader { get; set; }

        public uint Size { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
