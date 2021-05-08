using System;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoStakePoolMetadata
    {
        public string? Ticker { get; internal set; }

        public string? Name { get; internal set; }

        public string? Description { get; internal set; }

        public Uri? Website { get; internal set; }
    }
}
