using System;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoEpoch
    {
        public CardanoEpoch(CardanoEra era, ulong number, uint transactionCount, uint blockCount, CardanoAsset transactions, CardanoAsset fees, DateTime from, DateTime to)
        {
            this.Era = era;
            this.Number = number;
            this.TransactionsCount = transactionCount;
            this.BlockCount = blockCount;
            this.TransactionsTotal = transactions;
            this.Fees = fees;
            this.StartTime = from;
            this.EndTime = to;
            this.MaxSlots = era.SlotsPerEpoch;
        }

        public CardanoEra Era { get; internal set; }

        public ulong Number { get; internal set; }

        public uint TransactionsCount { get; internal set; }

        public uint BlockCount { get; set; }

        public CardanoAsset TransactionsTotal { get; internal set; }

        public CardanoAsset Fees { get; internal set; }

        public DateTime StartTime { get; internal set; }

        public DateTime EndTime { get; internal set; }

        public uint MaxSlots { get; set; }
    }

    public  class CurrentCardanoEpoch : CardanoEpoch
    {
        public CurrentCardanoEpoch(CardanoEra era, ulong number, uint transactionCount, uint blockCount, CardanoAsset transactions, CardanoAsset fees, DateTime from, DateTime to, uint currentSlot)
            : base(era, number, transactionCount, blockCount, transactions, fees, from, to)
        {
            this.CurrentSlot = currentSlot;
        }

        public uint CurrentSlot { get; set; }
    }
}
