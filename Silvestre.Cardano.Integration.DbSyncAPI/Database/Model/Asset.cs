using System.Text;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    internal class Asset
    {
        public Asset(byte[] value)
        {
            if (value.Length > 32) throw new ArgumentOutOfRangeException(nameof(value));

            this.Value = value;
        }

        public Asset(string address) : this(Encoding.ASCII.GetBytes(address))
        { }

        public byte[] Value { get; set; }
    }
}
