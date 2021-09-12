namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    internal class Epoch
    {
        public long Id { get; set; }

        public uint Number { get; set; }

        public decimal OutSum { get; set; }

        public ulong Fees { get; set; }

        public uint TransactionCount { get; set; }

        public uint BlockCount { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
