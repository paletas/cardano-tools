namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoAddress
    {
        public enum AddressKindEnum
        {
            Wallet,
            StakePool,
            Rewards
        }

        public CardanoAddress(string address, AddressKindEnum addressType)
        {
            this.Address = address;
            this.Type = addressType;
        }

        public string Address { get; set; }

        public AddressKindEnum Type { get; set; }
    }
}
