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
            var stakePools = await _databaseProxy.ListStakePools(request.Offset, request.Limit).ConfigureAwait(false);

            var reply = new ListStakePoolsReply();
            reply.Results.AddRange(stakePools.Select(sp => new StakePool
            {
                PoolAddress = sp.PoolAddress,
                RewardAddress = sp.RewardAddress,

                Pledge = sp.Pledge,
                Margin = (float)sp.Margin,
                FixedCost = sp.FixedCost,

                MetadataUrl = sp.MetadataUrl,
                ActiveSinceEpoch = sp.ActiveSinceEpochNumber,
                RetiringEpoch = sp.RetiringEpoch
            }));

            return reply;
        }

        public override async Task<GetStakePoolReply> GetStakePool(GetStakePoolRequest request, ServerCallContext context)
        {
            var stakePool = await _databaseProxy.GetStakePool(request.PoolAddress).ConfigureAwait(false);

            var reply = new GetStakePoolReply();

            if (stakePool != null)
            {
                reply.StakePool = new StakePool
                {
                    PoolAddress = stakePool.PoolAddress,
                    RewardAddress = stakePool.RewardAddress,

                    Pledge = stakePool.Pledge,
                    Margin = (float)stakePool.Margin,
                    FixedCost = stakePool.FixedCost,

                    MetadataUrl = stakePool.MetadataUrl,
                    ActiveSinceEpoch = stakePool.ActiveSinceEpochNumber,
                    RetiringEpoch = stakePool.RetiringEpoch
                };
            }

            return reply;
        }
    }
}
