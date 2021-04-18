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
    }
}
