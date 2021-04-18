using Google.Protobuf;
using Silvestre.Cardano.Integration.DbSync.Services;
using System;

namespace Silvestre.Cardano.Integration.DbSyncAPI
{
    internal static class Int128Extensions
    {
        public static Int128 ToInt128(this decimal value)
        {
            var intArray = decimal.GetBits(value);
            byte[] result = new byte[intArray.Length * sizeof(int)];
            Buffer.BlockCopy(intArray, 0, result, 0, result.Length);

            return new Int128 { Value = ByteString.CopyFrom(result) };
        }
    }
}
