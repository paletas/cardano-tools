namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoEra
    {
        public static CardanoEra MaryEra => new(432000);

        internal CardanoEra(uint slotsPerEpoch)
        {
            this.SlotsPerEpoch = slotsPerEpoch;
        }

        public uint SlotsPerEpoch { get; set; }
    }
}
