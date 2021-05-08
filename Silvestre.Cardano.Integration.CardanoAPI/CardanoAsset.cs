using System;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoAsset
    {
        public const string ADA_UNIT = "lovelace";
        public const int ADA_DECIMALPOINTER = 6;
        public const string PERCENTAGE_UNIT = "percentage";

        internal CardanoAsset(decimal quantity, string unit)
        {
            this.Quantity = quantity;
            this.Unit = unit;
        }

        internal CardanoAsset(ulong quantity, int decimalUnit, string unit)
        {
            this.Quantity = quantity / (decimal) Math.Pow(10, decimalUnit);
            this.Unit = unit;
        }

        public decimal Quantity { get; set; }

        public string Unit { get; set; }
    }
}
