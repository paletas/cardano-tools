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
        public static async Task<(ulong Total, IEnumerable<StakePool> StakePools)> ListStakePools(this DbConnection dbConnection, uint offset = 0, uint limit = 100)
        {
            const string QueryString =
				@"SELECT pool_hash.view AS PoolAddress,
					pool_update.pledge AS Pledge, pool_update.active_epoch_no AS ActiveSinceEpochNumber,
					pool_update.margin AS Margin, pool_update.fixed_cost AS FixedCost,
					pool_meta_data.url AS MetadataUrl,
					pool_retire.retiring_epoch AS RetiringEpoch,
					COUNT(*) OVER () AS TotalStakePools
				FROM public.pool_hash
					INNER JOIN public.pool_update ON pool_hash.id = pool_update.hash_id
					INNER JOIN public.pool_meta_data ON pool_update.registered_tx_id = pool_meta_data.registered_tx_id
					INNER JOIN public.pool_relay ON pool_relay.update_id = pool_update.id
					LEFT JOIN public.pool_retire ON pool_retire.hash_id = pool_hash.id
				WHERE pool_retire.retiring_epoch IS NULL OR pool_retire.retiring_epoch >= (SELECT MAX(no) FROM public.epoch)
				ORDER BY pool_hash.id ASC
				LIMIT @Limit
				OFFSET @Offset";

            var stakePools = await dbConnection.QueryAsync<StakePool>(QueryString, new { Limit = (int)limit, Offset = (int)offset }, commandType: System.Data.CommandType.Text).ConfigureAwait(false);

			return (stakePools.FirstOrDefault()?.TotalStakePools ?? 0, stakePools);
        }

        public static async Task<StakePool> GetStakePool(this DbConnection dbConnection, string poolAddress)
        {
            const string QueryString =
				@"SELECT pool_hash.view AS PoolAddress,
					pool_update.pledge AS Pledge, pool_update.active_epoch_no AS ActiveSinceEpochNumber,
					pool_update.margin AS Margin, pool_update.fixed_cost AS FixedCost,
					pool_meta_data.url AS MetadataUrl,
					pool_retire.retiring_epoch AS RetiringEpoch
				FROM public.pool_hash
					INNER JOIN public.pool_update ON pool_hash.id = pool_update.hash_id
					INNER JOIN public.pool_meta_data ON pool_update.registered_tx_id = pool_meta_data.registered_tx_id
					INNER JOIN public.pool_relay ON pool_relay.update_id = pool_update.id
					LEFT JOIN public.pool_retire ON pool_retire.hash_id = pool_hash.id
				WHERE (pool_retire.retiring_epoch IS NULL OR pool_retire.retiring_epoch >= (SELECT MAX(no) FROM public.epoch))
					AND pool_hash.view = @PoolAddress
				ORDER BY pool_hash.id ASC";

            return await dbConnection.QuerySingleOrDefaultAsync<StakePool>(QueryString, new { PoolAddress = poolAddress }, commandType: System.Data.CommandType.Text).ConfigureAwait(false);
        }
    }
}
