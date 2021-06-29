namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoEpochStatistics
    {
        public uint EpochNumber { get; set; }

        public ulong TotalStakePools { get; set; }

        public ulong TotalDelegations { get; set; }

        public CardanoAsset CirculatingSupply { get; set; }

        public CardanoAsset DelegatedSupply { get; set; }

        public CardanoAsset? Rewards { get; set; }

        public CardanoAsset? OrphanedRewards { get; set; }
    }
}
