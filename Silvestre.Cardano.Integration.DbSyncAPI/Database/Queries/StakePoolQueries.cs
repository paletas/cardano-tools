using Dapper;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries
{
    internal static class StakePoolQueries
    {
        public static async Task<(ulong Total, IEnumerable<StakePool> StakePools)> ListStakePools(this DbConnection dbConnection, uint? epochNo = null, uint offset = 0, uint limit = 100)
        {
			if (epochNo == null)
            {
				var epoch = await dbConnection.GetCurrentEpoch();
				epochNo = epoch.Number;
            }

            const string QueryString =
				@"SELECT pool_hash.view AS PoolAddress,
					pool_information.pledge AS Pledge, pool_information.active_epoch_no AS ActiveSinceEpochNumber,
					pool_information.margin AS Margin, pool_information.fixed_cost AS FixedCost,
					pool_information.url AS MetadataUrl,
					pool_delegation.total_stake As Delegation,
					pool_retire.retiring_epoch AS RetiringEpoch,
					COUNT(*) OVER () AS TotalStakePools
				FROM public.pool_hash
					INNER JOIN LATERAL (
						SELECT pool_update.hash_id, pool_update.pledge, pool_update.active_epoch_no, pool_update.margin, pool_update.fixed_cost, pool_meta_data.url
						FROM public.pool_update
							INNER JOIN public.pool_meta_data ON pool_update.registered_tx_id = pool_meta_data.registered_tx_id
						WHERE pool_hash.id = pool_update.hash_id AND pool_update.active_epoch_no <= @EpochNo
						ORDER BY pool_update.active_epoch_no DESC
						LIMIT 1
					) pool_information ON TRUE
					INNER JOIN LATERAL (
						SELECT SUM(amount) total_stake
						FROM public.epoch_stake
						WHERE epoch_stake.pool_id = pool_hash.id AND epoch_stake.epoch_no <= @EpochNo
						GROUP BY epoch_stake.pool_id
					) pool_delegation ON TRUE
					LEFT JOIN public.pool_retire ON pool_retire.hash_id = pool_hash.id
				WHERE pool_retire.retiring_epoch IS NULL OR pool_retire.retiring_epoch >= @EpochNo
				ORDER BY pool_hash.id ASC
				LIMIT @Limit
				OFFSET @Offset";

            var stakePools = await dbConnection.QueryAsync<StakePool>(QueryString, new { EpochNo = (int) epochNo, Limit = (int)limit, Offset = (int)offset }, commandType: System.Data.CommandType.Text).ConfigureAwait(false);

			return (stakePools.FirstOrDefault()?.TotalStakePools ?? 0, stakePools);
        }

        public static async Task<StakePool> GetStakePool(this DbConnection dbConnection, string poolAddress, uint? epochNo = null)
		{
			if (epochNo == null)
			{
				var epoch = await dbConnection.GetCurrentEpoch();
				epochNo = epoch.Number;
			}

			const string QueryString =
				@"SELECT pool_hash.view AS PoolAddress,
					pool_information.pledge AS Pledge, pool_information.active_epoch_no AS ActiveSinceEpochNumber,
					pool_information.margin AS Margin, pool_information.fixed_cost AS FixedCost,
					pool_information.url AS MetadataUrl,
					pool_delegation.total_stake As Delegation,
					pool_retire.retiring_epoch AS RetiringEpoch
				FROM public.pool_hash
					INNER JOIN LATERAL (
						SELECT pool_update.hash_id, pool_update.pledge, pool_update.active_epoch_no, pool_update.margin, pool_update.fixed_cost, pool_meta_data.url
						FROM public.pool_update
							INNER JOIN public.pool_meta_data ON pool_update.registered_tx_id = pool_meta_data.registered_tx_id
						WHERE pool_hash.id = pool_update.hash_id AND pool_update.active_epoch_no <= @EpochNo
						ORDER BY pool_update.active_epoch_no DESC
						LIMIT 1
					) pool_information ON TRUE
					INNER JOIN LATERAL (
						SELECT SUM(amount) total_stake
						FROM public.epoch_stake
						WHERE epoch_stake.pool_id = pool_hash.id AND epoch_stake.epoch_no <= @EpochNo
						GROUP BY epoch_stake.pool_id
					) pool_delegation ON TRUE
					LEFT JOIN public.pool_retire ON pool_retire.hash_id = pool_hash.id
				WHERE (pool_retire.retiring_epoch IS NULL OR pool_retire.retiring_epoch >= @EpochNo)
					AND pool_hash.view = @PoolAddress";

            return await dbConnection.QuerySingleOrDefaultAsync<StakePool>(QueryString, new { PoolAddress = poolAddress, EpochNo = (int) epochNo }, commandType: System.Data.CommandType.Text).ConfigureAwait(false);
        }
    }
}
