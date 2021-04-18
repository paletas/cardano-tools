using System;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoStakePool
    {
        public CardanoAddress PoolAddress { get; internal set; }

        public string? Ticker { get; internal set; }

        public string? Name { get; internal set; }

        public string? Description { get; internal set; }

        public Uri MetadataUrl { get; internal set; }

        public Uri? Website { get; internal set; }

        public CardanoAsset Maintenance { get; internal set; }

        public CardanoAsset Margin { get; internal set; }

        public CardanoAsset Pledge { get; internal set; }

        public CardanoAddress RewardsAddress { get; internal set; }
    }
}
