using Silvestre.Cardano.Integration.DbSync.Services;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    internal static class Int128Extensions
    {
        public static decimal ToDecimal(this int128 int128, int decimalUnit)
        {
            var value = int128.Value.ToByteArray();
            if (value.Length != 16) throw new Exception("A decimal must be created from exactly 16 bytes");

            int[] bits = new int[4];
            for (int i = 0; i <= 15; i += 4)
            {
                bits[i / 4] = BitConverter.ToInt32(value, i);
            }

            var decimalValue = new decimal(bits);
            decimalValue /= (decimal)Math.Pow(10, decimalUnit);
            return decimalValue;
        }
    }
}
