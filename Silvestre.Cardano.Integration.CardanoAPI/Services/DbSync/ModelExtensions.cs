namespace Silvestre.Cardano.Integration.CardanoAPI.Services.DbSync
{
    internal static class ModelExtensions
    {
        public static Int128 ToInt128(this Silvestre.Cardano.Integration.DbSync.Services.Int128 serviceInt128)
        {
            return new Int128(serviceInt128.Value.ToByteArray());
        }
    }
}
