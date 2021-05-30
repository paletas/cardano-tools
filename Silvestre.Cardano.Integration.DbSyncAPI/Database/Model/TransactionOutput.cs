namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    public class TransactionOutput : Transaction
    {
        public string AddressTo { get; set; }

        public ulong Amount { get; set; }
    }
}
