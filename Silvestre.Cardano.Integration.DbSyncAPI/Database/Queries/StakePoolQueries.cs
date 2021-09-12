using Dapper;
using Silvestre.Cardano.Integration.DbSyncAPI.Database.Model;
using System.Data.Common;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Database.Queries
{
    internal static class StakePoolQueries
    {
        private record SearchStakePoolResult(long PoolId, long TotalStakePools);
        private record StakePoolMetadataUpdates(long PoolId, string PoolAddress, decimal Pledge, long ActiveSinceEpochNumber, double Margin, decimal FixedCost, string MetadataUrl);

        public static async Task<(ulong Total, IEnumerable<StakePool> StakePools)> ListStakePools(this DbConnection dbConnection, CancellationToken cancellationToken, uint? epochNo = null, uint offset = 0, uint limit = 100)
        {
            if (epochNo == null)
            {
                var epoch = await dbConnection.GetCurrentEpoch(cancellationToken);
                epochNo = epoch.Number;
            }

            const string SearchStakePoolsQueryString =
                @"SELECT pool_update.hash_id PoolId, COUNT(*) OVER () AS TotalStakePools
				FROM public.pool_update 
					LEFT JOIN public.pool_retire ON pool_update.hash_id = pool_retire.hash_id
				WHERE pool_update.active_epoch_no <= @EpochNo
					AND (pool_retire.retiring_epoch IS NULL OR pool_retire.retiring_epoch >= @EpochNo)
				GROUP BY pool_update.hash_id
				ORDER BY pool_update.hash_id ASC
				LIMIT @Limit
				OFFSET @Offset";

            const string StakePoolUpdateDetails =
                @"SELECT pool_hash.id PoolId, pool_hash.view AS PoolAddress,
					pool_update.pledge AS Pledge, pool_update.active_epoch_no AS ActiveSinceEpochNumber,
					pool_update.margin AS Margin, pool_update.fixed_cost AS FixedCost,
					pool_metadata_ref.url AS MetadataUrl
				FROM public.pool_hash
					INNER JOIN public.pool_update ON pool_hash.id = pool_update.hash_id
	                LEFT JOIN public.pool_metadata_ref ON pool_update.registered_tx_id = pool_metadata_ref.registered_tx_id
				WHERE pool_hash.id = @PoolId";

            const string StakeQueryString = @"
				SELECT SUM(amount) TotalStake
				FROM public.epoch_stake
				WHERE epoch_stake.pool_id = @PoolId AND epoch_no <= @EpochNo
			";

            var searchCommand = new CommandDefinition(SearchStakePoolsQueryString, new { EpochNo = (int)epochNo, Limit = (int)limit, Offset = (int)offset }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
            var stakePoolsSearch = await dbConnection.QueryAsync<SearchStakePoolResult>(searchCommand).ConfigureAwait(false);

            var stakePools = stakePoolsSearch.Select(pool => new StakePool
            {
                PoolId = pool.PoolId
            }).ToArray();

            foreach (var stakePool in stakePools)
            {
                var updatesCommand = new CommandDefinition(StakePoolUpdateDetails, new { PoolId = stakePool.PoolId }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
                var stakePoolUpdates = await dbConnection.QueryAsync<StakePoolMetadataUpdates>(updatesCommand).ConfigureAwait(false);
                if (stakePoolUpdates.Any() == false) throw new InvalidOperationException($"There should be atleast an update for the pool id {stakePool.PoolId}");

                foreach (var poolUpdate in stakePoolUpdates.Where(upd => upd.ActiveSinceEpochNumber <= epochNo).OrderBy(upd => upd.ActiveSinceEpochNumber))
                {
                    stakePool.PoolAddress = poolUpdate.PoolAddress;

                    if (stakePool.ActiveSinceEpochNumber == default)
                        stakePool.ActiveSinceEpochNumber = (ulong)poolUpdate.ActiveSinceEpochNumber;

                    stakePool.Pledge = (ulong)poolUpdate.Pledge;
                    stakePool.Margin = poolUpdate.Margin;
                    stakePool.FixedCost = (ulong)poolUpdate.FixedCost;
                    stakePool.MetadataUrl = poolUpdate.MetadataUrl ?? stakePool.MetadataUrl;
                }

                var delegationCommand = new CommandDefinition(StakeQueryString, new { PoolId = stakePool.PoolId, EpochNo = (int)epochNo }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
                var delegation = await dbConnection.QuerySingleOrDefaultAsync<ulong>(delegationCommand);
                stakePool.Delegation = delegation;
            };

            return ((ulong)(stakePoolsSearch.FirstOrDefault()?.TotalStakePools ?? 0), stakePools);
        }

        public static async Task<StakePool> GetStakePool(this DbConnection dbConnection, CancellationToken cancellationToken, string poolAddress, uint? epochNo = null)
        {
            if (epochNo == null)
            {
                var epoch = await dbConnection.GetCurrentEpoch(cancellationToken);
                epochNo = epoch.Number;
            }

            const string StakePoolUpdateDetails =
                @"SELECT pool_hash.id PoolId, pool_hash.view AS PoolAddress,
	                pool_update.pledge AS Pledge, pool_update.active_epoch_no AS ActiveSinceEpochNumber,
	                pool_update.margin AS Margin, pool_update.fixed_cost AS FixedCost,
	                pool_metadata_ref.url AS MetadataUrl
                FROM public.pool_hash
	                INNER JOIN public.pool_update ON pool_hash.id = pool_update.hash_id
	                LEFT JOIN public.pool_metadata_ref ON pool_update.registered_tx_id = pool_metadata_ref.registered_tx_id
                WHERE pool_hash.view =  @PoolAddress";

            const string StakeQueryString = @"
				SELECT SUM(amount) TotalStake
				FROM public.epoch_stake
				WHERE epoch_stake.pool_id = @PoolId AND epoch_no <= @EpochNo
			";

            var updatesCommand = new CommandDefinition(StakePoolUpdateDetails, new { PoolAddress = poolAddress }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
            var stakePoolUpdates = await dbConnection.QueryAsync<StakePoolMetadataUpdates>(updatesCommand).ConfigureAwait(false);
            if (stakePoolUpdates.Count() == 0) return null;

            var stakePool = new StakePool { PoolAddress = poolAddress };

            foreach (var poolUpdate in stakePoolUpdates.Where(upd => upd.ActiveSinceEpochNumber <= epochNo).OrderBy(upd => upd.ActiveSinceEpochNumber))
            {
                stakePool.PoolId = poolUpdate.PoolId;

                if (stakePool.ActiveSinceEpochNumber == default)
                    stakePool.ActiveSinceEpochNumber = (ulong)poolUpdate.ActiveSinceEpochNumber;

                stakePool.Pledge = (ulong)poolUpdate.Pledge;
                stakePool.Margin = poolUpdate.Margin;
                stakePool.FixedCost = (ulong)poolUpdate.FixedCost;
                stakePool.MetadataUrl = poolUpdate.MetadataUrl ?? stakePool.MetadataUrl;
            }

            var delegationCommand = new CommandDefinition(StakeQueryString, new { PoolId = stakePool.PoolId, EpochNo = (int)epochNo }, commandType: System.Data.CommandType.Text, cancellationToken: cancellationToken);
            var delegation = await dbConnection.QuerySingleOrDefaultAsync<ulong>(delegationCommand);
            stakePool.Delegation = delegation;

            return stakePool;
        }
    }
}
