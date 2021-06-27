using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Silvestre.Cardano.Integration.DbSync.Services;
using System.Threading.Tasks;

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
            var currentEpoch = await this._databaseProxy.GetCurrentEpoch().ConfigureAwait(false);

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
            var epoch = await this._databaseProxy.GetEpoch(request.EpochNumber).ConfigureAwait(false);

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

        public override async Task<GetEpochStatisticsReply> GetEpochStatistics(GetEpochStatisticsRequest request, ServerCallContext context)
        {
            var epochStatistics = await this._databaseProxy.GetEpochStatistics(request.EpochNumber).ConfigureAwait(false);

            return new GetEpochStatisticsReply
            {
                Statistics = new EpochStatistics
                {
                    Number = epochStatistics.EpochNumber,
                    CirculatingSupply = epochStatistics.CirculatingSupply,
                    DelegatedSupply = epochStatistics.DelegatedSupply,
                    Rewards = epochStatistics.Rewards ?? 0,
                    OrphanedRewards = epochStatistics.OprhanedRewards ?? 0,
                    RewardsCalculated = epochStatistics.Rewards.HasValue || epochStatistics.OprhanedRewards.HasValue,
                    TotalStakePools = epochStatistics.TotalStakePools,
                    TotalDelegations =  epochStatistics.TotalDelegations
                }
            };
        }
    }
}
