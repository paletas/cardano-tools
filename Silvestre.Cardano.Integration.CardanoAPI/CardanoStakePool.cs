using System;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoStakePool : CardanoStakePoolMetadata
    {
        public CardanoAddress PoolAddress { get; internal set; }

        public Uri MetadataUrl { get; internal set; }

        public CardanoAsset Maintenance { get; internal set; }

        public CardanoAsset Margin { get; internal set; }

        public CardanoAsset Pledge { get; internal set; }

        public CardanoAddress RewardsAddress { get; internal set; }
    }
}
