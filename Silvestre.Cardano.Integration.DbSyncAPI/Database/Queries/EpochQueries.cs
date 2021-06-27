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

        public static async Task<Epoch> GetEpoch(this DbConnection sqlConnection, uint epochNumber)
        {
            const string QueryString =
                @"SELECT id as Id, no as Number, out_sum as OutSum, fees as Fees, tx_count as TransactionCount, blk_Count as BlockCount, start_time as StartTime, end_time as EndTime 
                FROM public.epoch
                WHERE no = @EpochNumber";

            var epochData = await sqlConnection.QuerySingleAsync<Epoch>(QueryString, new { EpochNumber = (long) epochNumber }, commandType: System.Data.CommandType.Text).ConfigureAwait(false);
            epochData.StartTime = epochData.StartTime.ToUniversalTime();
            epochData.EndTime = epochData.EndTime.ToUniversalTime();

            return epochData;
        }

        public static async Task<EpochStatistics> GetEpochStatistics(this DbConnection sqlConnection, uint epochNumber)
        {
            const string QueryString =
                @"SELECT epoch.no AS EpochNumber, delegation.delegated_total AS TotalDelegations, delegation.delegated_supply AS DelegatedSupply, stakepools.total AS TotalStakePools, rewards.orphanedrewards_total AS OrphanedRewards, rewards.rewards_total AS Rewards, onchain.current_supply AS CirculatingSupply
                FROM public.epoch 
	                INNER JOIN LATERAL (
		                SELECT SUM(amount) delegated_supply, COUNT(addr_id) delegated_total
		                FROM public.epoch_stake
		                WHERE epoch.no = epoch_stake.epoch_no
	                ) delegation ON TRUE
	                INNER JOIN LATERAL (
		                SELECT COUNT(DISTINCT pool_hash.id) total
		                FROM public.pool_hash 
			                INNER JOIN public.pool_update ON pool_hash.id = pool_update.hash_id
							LEFT JOIN public.pool_retire ON  pool_hash.id = pool_retire.hash_id
		                WHERE pool_update.active_epoch_no <= epoch.no AND (pool_retire.id IS NULL OR pool_retire.retiring_epoch > epoch.no)
	                ) stakepools ON TRUE
	                INNER JOIN LATERAL (
		                SELECT SUM(orphaned_reward.amount) orphanedrewards_total, SUM(reward.amount) rewards_total
		                FROM public.reward, public.orphaned_reward
		                WHERE orphaned_reward.epoch_no = epoch.no OR reward.epoch_no = epoch.no
	                ) rewards ON TRUE
	                INNER JOIN LATERAL (
		                SELECT SUM(value) as current_supply 
		                FROM public.epoch AS txout_epoch
			                INNER JOIN public.block ON txout_epoch.no = block.epoch_no
			                INNER JOIN public.tx ON block.id = tx.block_id
			                INNER JOIN public.tx_out AS tx_outer ON tx_outer.tx_id = tx.id
		                WHERE NOT EXISTS ( 
			                SELECT tx_out.id 
			                FROM public.tx_out
				                INNER JOIN public.tx_in ON tx_out.tx_id = tx_in.tx_out_id AND tx_out.index = tx_in.tx_out_index
				                INNER JOIN public.tx ON tx_in.tx_in_id = tx.id
				                INNER JOIN public.block ON block.id = tx.block_id
				                INNER JOIN public.epoch AS txin_epoch ON txin_epoch.no = block.epoch_no
			                WHERE tx_outer.id = tx_out.id AND txin_epoch.no <= epoch.no
  		                ) AND txout_epoch.no <= epoch.no
	                ) onchain ON TRUE
                WHERE epoch.no = @EpochNumber";

            return await sqlConnection.QuerySingleAsync<EpochStatistics>(QueryString, new { EpochNumber = (long)epochNumber }, commandType: System.Data.CommandType.Text).ConfigureAwait(false);
        }
    }
}
