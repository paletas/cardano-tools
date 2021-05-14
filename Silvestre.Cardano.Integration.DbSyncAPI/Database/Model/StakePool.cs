namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    internal class StakePool
    {
        public string PoolAddress { get; set; }

        public ulong Pledge { get; set; }

        public uint ActiveSinceEpochNumber { get; set; }

        public decimal Margin { get; set; }

        public ulong FixedCost { get; set; }

        public string MetadataUrl { get; set; }

        public uint RetiringEpoch { get; set; }

        public ulong TotalStakePools { get; set; }
    }
}
