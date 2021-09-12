using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Silvestre.Cardano.Integration.DbSync.Services;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Services
{
    internal class EpochService : Epochs.EpochsBase
    {
        private readonly ILogger<EpochService> _logger;
        private readonly DatabaseProxy _databaseProxy;

        public EpochService(ILogger<EpochService> logger, DatabaseProxy databaseProxy)
        {
            _logger = logger;
            _databaseProxy = databaseProxy;
        }

        public override async Task<CurrentEpochReply> GetCurrentEpoch(CurrentEpochRequest request, ServerCallContext context)
        {
            var currentEpoch = await this._databaseProxy.GetCurrentEpoch(context.CancellationToken).ConfigureAwait(false);

            if (currentEpoch == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Epoch not found"));

            return new CurrentEpochReply
            {
                Epoch = new Epoch
                {
                    Number = currentEpoch.Number,
                    StartTime = currentEpoch.StartTime.ToTimestamp(),
                    EndTime = currentEpoch.EndTime.ToTimestamp(),
                    TransactionCount = currentEpoch.TransactionCount,
                    BlockCount = currentEpoch.BlockCount,
                    Fees = currentEpoch.Fees,
                    OutSum = currentEpoch.OutSum.ToInt128()
                }
            };
        }

        public override async Task<GetEpochReply> GetEpoch(GetEpochRequest request, ServerCallContext context)
        {
            var epoch = await this._databaseProxy.GetEpoch(context.CancellationToken, request.EpochNumber).ConfigureAwait(false);
            if (epoch == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Epoch not found"), new Metadata { { "EpochNumber", request.EpochNumber.ToString() } });

            return new GetEpochReply
            {
                Epoch = new Epoch
                {
                    Number = epoch.Number,
                    StartTime = epoch.StartTime.ToTimestamp(),
                    EndTime = epoch.EndTime.ToTimestamp(),
                    TransactionCount = epoch.TransactionCount,
                    BlockCount = epoch.BlockCount,
                    Fees = epoch.Fees,
                    OutSum = epoch.OutSum.ToInt128()
                }
            };
        }

        public override async Task<GetEpochDelegationStatisticsReply> GetEpochDelegationStatistics(GetEpochDelegationStatisticsRequest request, ServerCallContext context)
        {
            var epochStatistics = await this._databaseProxy.GetEpochDelegationStatistics(context.CancellationToken, request.EpochNumber).ConfigureAwait(false);

            return new GetEpochDelegationStatisticsReply
            {
                EpochNumber = epochStatistics.EpochNumber,
                Rewards = epochStatistics.Rewards ?? 0,
                OrphanedRewards = epochStatistics.OprhanedRewards ?? 0,
                RewardsCalculated = epochStatistics.Rewards.HasValue || epochStatistics.OprhanedRewards.HasValue,
                TotalStakePools = epochStatistics.TotalStakePools,
                TotalDelegations = epochStatistics.TotalDelegations
            };
        }

        public override async Task<GetEpochSupplyStatisticsReply> GetEpochSupplyStatistics(GetEpochSupplyStatisticsRequest request, ServerCallContext context)
        {
            var epochStatistics = await this._databaseProxy.GetEpochCirculationStatistics(context.CancellationToken, request.EpochNumber).ConfigureAwait(false);

            return new GetEpochSupplyStatisticsReply
            {
                EpochNumber = epochStatistics.EpochNumber,
                CirculatingSupply = epochStatistics.CirculatingSupply,
                DelegatedSupply = epochStatistics.DelegatedSupply
            };
        }
    }
}
