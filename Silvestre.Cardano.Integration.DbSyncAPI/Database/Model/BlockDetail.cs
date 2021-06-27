namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Model
{
    internal class BlockDetail : Block
    {
        public ulong  TotalFees { get; set; }

        public decimal TotalOutSum { get; set; }
    }
}
