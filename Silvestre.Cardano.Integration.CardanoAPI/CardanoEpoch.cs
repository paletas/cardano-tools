using System;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoEpoch
    {
        public CardanoEpoch(CardanoEra era, ulong number, uint transactionCount, uint blockCount, Int128  lovelaceCirculated, CardanoAsset fees, DateTime from, DateTime to)
        {
            this.Era = era;
            this.Number = number;
            this.TransactionCount = transactionCount;
            this.BlockCount = blockCount;
            this.LovelaceCirculation = lovelaceCirculated;
            this.Fees = fees;
            this.StartTime = from;
            this.EndTime = to;
        }

        public CardanoEra Era { get; internal set; }

        public ulong Number { get; internal set; }

        public uint TransactionCount { get; internal set; }

        public uint BlockCount { get; set; }

        public Int128 LovelaceCirculation { get; internal set; }

        public CardanoAsset Fees { get; internal set; }

        public DateTime StartTime { get; internal set; }

        public DateTime EndTime { get; internal set; }
    }

    public  class CurrentCardanoEpoch : CardanoEpoch
    {
        public CurrentCardanoEpoch(CardanoEra era, ulong number, uint transactionCount, uint blockCount, Int128 lovelaceCirculated, CardanoAsset fees, DateTime from, DateTime to, uint currentSlot)
            : base(era, number, transactionCount, blockCount, lovelaceCirculated, fees, from, to)
        {
            this.CurrentSlot = currentSlot;
            this.MaxSlots = era.SlotsPerEpoch;
        }

        public uint CurrentSlot { get; set; }

        public uint MaxSlots { get; set; }
    }
}
