using System.Text;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public struct Int128
    {
        public Int128(byte[] value)
        {
            this.Value = value;
        }

        public Int128(string value) : this(Encoding.ASCII.GetBytes(value))
        { }

        public byte[] Value { get; set; }
    }
}
