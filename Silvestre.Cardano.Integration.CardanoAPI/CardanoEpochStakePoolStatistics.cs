namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoEpochStakePoolStatistics
    {
        public uint EpochNumber { get; set; }

        public ulong TotalStakePools { get; set; }

        public ulong TotalDelegations { get; set; }

        public CardanoAsset? Rewards { get; set; }

        public CardanoAsset? OrphanedRewards { get; set; }
    }
}
