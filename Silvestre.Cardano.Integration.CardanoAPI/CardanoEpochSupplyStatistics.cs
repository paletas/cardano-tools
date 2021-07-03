namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoEpochSupplyStatistics
    {
        public uint EpochNumber { get; set; }

        public CardanoAsset CirculatingSupply { get; set; }

        public CardanoAsset StakedSupply { get; set; }
    }
}
