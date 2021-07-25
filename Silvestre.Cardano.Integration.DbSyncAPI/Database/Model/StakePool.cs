namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    internal class StakePool
    {
        public long PoolId { get; set; }

        public string PoolAddress { get; set; }

        public ulong Pledge { get; set; }

        public ulong ActiveSinceEpochNumber { get; set; }

        public double Margin { get; set; }

        public ulong FixedCost { get; set; }

        public string MetadataUrl { get; set; }

        public ulong Delegation { get; set; }
    }
}
