using Dapper;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using System.Data.Common;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries
{
    internal static class EpochQueries
    {
        public static async Task<Epoch> GetCurrentEpoch(this DbConnection sqlConnection, CancellationToken cancellationToken)
        {
            const string QueryString =
                @"SELECT id as Id, no as Number, out_sum as OutSum, fees as Fees, tx_count as TransactionCount, blk_Count as BlockCount, start_time as StartTime, end_time as EndTime 
                FROM public.epoch
                ORDER BY no DESC
                LIMIT 1";

            var epochCommand = new CommandDefinition(QueryString, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
            var epochData = await sqlConnection.QuerySingleAsync<Epoch>(epochCommand).ConfigureAwait(false);
            epochData.StartTime = epochData.StartTime.ToUniversalTime();
            epochData.EndTime = epochData.EndTime.ToUniversalTime();

            return epochData;
        }

        public static async Task<Epoch> GetEpoch(this DbConnection sqlConnection, CancellationToken cancellationToken, uint epochNumber)
        {
            const string QueryString =
                @"SELECT id as Id, no as Number, out_sum as OutSum, fees as Fees, tx_count as TransactionCount, blk_Count as BlockCount, start_time as StartTime, end_time as EndTime 
                FROM public.epoch
                WHERE no = @EpochNumber";

            var epochCommand = new CommandDefinition(QueryString, new { EpochNumber = (long)epochNumber }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
            var epochData = await sqlConnection.QuerySingleOrDefaultAsync<Epoch>(QueryString, new { EpochNumber = (long)epochNumber }, commandType: System.Data.CommandType.Text).ConfigureAwait(false);
            if (epochData == default(Epoch)) return null;

            epochData.StartTime = epochData.StartTime.ToUniversalTime();
            epochData.EndTime = epochData.EndTime.ToUniversalTime();

            return epochData;
        }

        public static async Task<EpochStatistics> GetEpochDelegationStatistics(this DbConnection sqlConnection, CancellationToken cancellationToken, uint epochNumber)
        {
            const string QueryString =
				@"SELECT epoch.no AS EpochNumber, delegation.delegated_total AS TotalDelegations, stakepools.total AS TotalStakePools, orphanedrewards.total AS OrphanedRewards, rewards.total AS Rewards
				FROM public.epoch 
					INNER JOIN LATERAL (
						SELECT COUNT(id) delegated_total
						FROM public.epoch_stake
						WHERE epoch.no = epoch_stake.epoch_no
					) delegation ON TRUE
					INNER JOIN LATERAL (
						SELECT COUNT(DISTINCT pool_update.hash_id) total
						FROM public.pool_update
							LEFT JOIN public.pool_retire ON  pool_update.id = pool_retire.hash_id
						WHERE pool_update.active_epoch_no <= epoch.no AND (pool_retire.id IS NULL OR pool_retire.retiring_epoch >= epoch.no)
					) stakepools ON TRUE
					INNER JOIN LATERAL (
						SELECT SUM(reward.amount) total
						FROM public.reward
						WHERE epoch.no = reward.earned_epoch
					) rewards ON TRUE
					INNER JOIN LATERAL (
						SELECT SUM(orphaned_reward.amount) total
						FROM public.orphaned_reward
						WHERE epoch.no = orphaned_reward.epoch_no
					) orphanedrewards ON TRUE
				WHERE epoch.no = @EpochNumber";

            var epochCommand = new CommandDefinition(QueryString, new { EpochNumber = (long)epochNumber }, commandTimeout: 180, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
            return await sqlConnection.QuerySingleAsync<EpochStatistics>(epochCommand).ConfigureAwait(false);
        }

        public static async Task<EpochStatistics> GetEpochCirculationStatistics(this DbConnection sqlConnection, CancellationToken cancellationToken, uint epochNumber)
        {
            const string QueryString =
				@"SELECT epoch.no AS EpochNumber, delegation.delegated_supply AS DelegatedSupply, onchain.current_supply AS CirculatingSupply
				FROM public.epoch 
					INNER JOIN LATERAL (
						SELECT SUM(amount) delegated_supply
						FROM public.epoch_stake
						WHERE epoch_stake.epoch_no = epoch.no
					) delegation ON TRUE
					INNER JOIN LATERAL (
						SELECT MAX(tx.id) tx_id
						FROM public.block 
							INNER JOIN public.tx ON block.id = tx.block_id
						WHERE block.epoch_no = epoch.no
					) epoch_lasttransaction ON TRUE
					INNER JOIN LATERAL (
						SELECT SUM(value) as current_supply 
						FROM public.tx_out
						WHERE tx_out.tx_id <= epoch_lasttransaction.tx_id
							AND NOT EXISTS (			
								SELECT tx_in.tx_in_id AS tx_spent_id
								FROM public.tx_in 
								WHERE tx_out.tx_id = tx_in.tx_out_id AND tx_out.index = tx_in.tx_out_index AND tx_in.tx_in_id <= epoch_lasttransaction.tx_id
							)
					) onchain ON TRUE
				WHERE epoch.no = @EpochNumber";

            var epochCommand = new CommandDefinition(QueryString, new { EpochNumber = (long)epochNumber }, commandTimeout: 180, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
            return await sqlConnection.QuerySingleAsync<EpochStatistics>(epochCommand).ConfigureAwait(false);
        }
    }
}
