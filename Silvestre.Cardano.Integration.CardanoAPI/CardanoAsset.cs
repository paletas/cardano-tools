namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoAsset
    {
        public const string ADA_UNIT = "lovelace";
        public const int ADA_DECIMALPOINTER = 18;
        public const string PERCENTAGE_UNIT = "percentage";

        internal CardanoAsset(decimal quantity, string unit)
        {
            this.Quantity = quantity;
            this.Unit = unit;
        }

        internal CardanoAsset(ulong quantity, int decimalUnit, string unit)
        {
            this.Quantity = quantity / (decimal)(decimalUnit * 10);
            this.Unit = unit;
        }

        public decimal Quantity { get; set; }

        public string Unit { get; set; }
    }
}
