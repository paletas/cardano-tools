namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    internal class EpochStatistics
    {
        public uint EpochNumber { get; set; }

        public ulong TotalStakePools { get; set; }

        public ulong TotalDelegations { get; set; }

        public ulong CirculatingSupply { get; set; }

        public ulong DelegatedSupply { get; set; }

        public ulong? Rewards { get; set; }

        public ulong? OprhanedRewards { get; set; }
    }
}
