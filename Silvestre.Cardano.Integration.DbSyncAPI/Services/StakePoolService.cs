using Grpc.Core;
using Microsoft.Extensions.Logging;
using Silvestre.Cardano.Integration.DbSync.Services;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Services
{
    internal class StakePoolService : StakePools.StakePoolsBase
    {
        private readonly ILogger<StakePoolService> _logger;
        private readonly DatabaseProxy _databaseProxy;

        public StakePoolService(ILogger<StakePoolService> logger, DatabaseProxy databaseProxy)
        {
            _logger = logger;
            _databaseProxy = databaseProxy;
        }

        public override async Task<ListStakePoolsReply> ListStakePools(ListStakePoolsRequest request, ServerCallContext context)
        {
            var listStakePools = await _databaseProxy.ListStakePools(context.CancellationToken, offset: request.Offset, limit: request.Limit).ConfigureAwait(false);

            var reply = new ListStakePoolsReply { Total = listStakePools.Total, From = request.Offset };
            reply.StakePools.AddRange(listStakePools.StakePools.Select(stakePool => new StakePool
            {
                PoolAddress = stakePool.PoolAddress,

                Pledge = stakePool.Pledge,
                Margin = (float)stakePool.Margin,
                FixedCost = stakePool.FixedCost,
                Delegation = stakePool.Delegation,

                MetadataUrl = stakePool.MetadataUrl,
                ActiveSinceEpoch = stakePool.ActiveSinceEpochNumber
            }));

            return reply;
        }

        public override async Task<ListStakePoolsReply> ListStakePoolsByEpoch(ListStakePoolsByEpochRequest request, ServerCallContext context)
        {
            var listStakePools = await _databaseProxy.ListStakePools(context.CancellationToken, request.EpochNumber, request.Offset, request.Limit).ConfigureAwait(false);

            var reply = new ListStakePoolsReply { Total = listStakePools.Total, From = request.Offset };
            reply.StakePools.AddRange(listStakePools.StakePools.Select(stakePool => new StakePool
            {
                PoolAddress = stakePool.PoolAddress,

                Pledge = stakePool.Pledge,
                Margin = (float)stakePool.Margin,
                FixedCost = stakePool.FixedCost,
                Delegation = stakePool.Delegation,

                MetadataUrl = stakePool.MetadataUrl,
                ActiveSinceEpoch = stakePool.ActiveSinceEpochNumber
            }));

            return reply;
        }

        public override async Task<GetStakePoolReply> GetStakePool(GetStakePoolRequest request, ServerCallContext context)
        {
            var stakePool = await _databaseProxy.GetStakePool(context.CancellationToken, request.PoolAddress).ConfigureAwait(false);

            var reply = new GetStakePoolReply();

            if (stakePool != null)
            {
                reply.StakePool = new StakePool
                {
                    PoolAddress = stakePool.PoolAddress,

                    Pledge = stakePool.Pledge,
                    Margin = (float)stakePool.Margin,
                    FixedCost = stakePool.FixedCost,
                    Delegation = stakePool.Delegation,

                    MetadataUrl = stakePool.MetadataUrl,
                    ActiveSinceEpoch = stakePool.ActiveSinceEpochNumber
                };
            }

            return reply;
        }
    }
}
