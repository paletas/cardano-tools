using System;
using System.Text;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    internal struct Address
    {
        public Address(byte[] address)
        {
            if (address.Length > 29) throw new ArgumentOutOfRangeException(nameof(address));

            this.Value = address;
        }

        public Address(string address) : this (Encoding.ASCII.GetBytes(address))
        { }

        public byte[] Value { get; set; }
    }
}
