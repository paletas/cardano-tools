using Dapper;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using System.Data.Common;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries
{
    internal static class BlockQueries
    {
        public static async Task<Block> GetLatestBlockForEpoch(this DbConnection sqlConnection, uint epochNumber)
        {
            const string QueryString =
                @"SELECT id as Id, hash as Hash, epoch_no as EpochNumber, slot_no as SlotNumber, epoch_slot_no as EpochSlotNumber, block_no as BlockNumber, previous_id  as PreviousID, slot_leader_id as SlotLeaderId, size as Size, ""time"" as Timestamp
                FROM public.block
                WHERE epoch_no = @EpochNumber
                ORDER BY id DESC
                LIMIT 1";

            var blockData = await sqlConnection.QuerySingleAsync<Block>(QueryString, new { EpochNumber = (long) epochNumber }, commandType: System.Data.CommandType.Text).ConfigureAwait(false);
            blockData.Timestamp = blockData.Timestamp.ToUniversalTime();

            return blockData;
        }

        public static async Task<Block> GetLatestBlock(this DbConnection sqlConnection)
        {
            const string QueryString =
                @"SELECT id as Id, hash as Hash, epoch_no as EpochNumber, slot_no as SlotNumber, epoch_slot_no as EpochSlotNumber, block_no as BlockNumber, previous_id  as PreviousID, slot_leader_id as SlotLeaderId, size as Size, ""time"" as Timestamp
                FROM public.block
                ORDER BY id DESC
                LIMIT 1";

            var blockData = await sqlConnection.QuerySingleAsync<Block>(QueryString, commandType: System.Data.CommandType.Text).ConfigureAwait(false);
            blockData.Timestamp = blockData.Timestamp.ToUniversalTime();

            return blockData;
        }
    }
}
