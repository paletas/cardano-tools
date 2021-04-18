using Dapper;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using System.Data.Common;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries
{
    internal static class EpochQueries
    {
        public static async Task<Epoch> GetCurrentEpoch(this DbConnection sqlConnection)
        {
            const string QueryString =
                @"SELECT id as Id, no as Number, out_sum as OutSum, fees as Fees, tx_count as TransactionCount, blk_Count as BlockCount, start_time as StartTime, end_time as EndTime 
                FROM public.epoch
                ORDER BY no DESC
                LIMIT 1";

            var epochData = await sqlConnection.QuerySingleAsync<Epoch>(QueryString, commandType: System.Data.CommandType.Text).ConfigureAwait(false);
            epochData.StartTime = epochData.StartTime.ToUniversalTime();
            epochData.EndTime = epochData.EndTime.ToUniversalTime();

            return epochData;
        }
    }
}
