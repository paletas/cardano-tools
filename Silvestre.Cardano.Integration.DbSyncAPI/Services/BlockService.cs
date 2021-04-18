using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Silvestre.Cardano.Integration.DbSync.Services;
using System;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.DbSyncAPI.Services
{
    internal class BlockService : Blocks.BlocksBase
    {
        private readonly ILogger<BlockService> _logger;
        private readonly DatabaseProxy _databaseProxy;

        public BlockService(ILogger<BlockService> logger, DatabaseProxy databaseProxy)
        {
            _logger = logger;
            _databaseProxy = databaseProxy;
        }

        public override async Task<LatestBlockReply> GetLatestBlock(LatestBlockRequest request, ServerCallContext context)
        {
            var latestBlock = await this._databaseProxy.GetLatestBlock(request.EpochNumber).ConfigureAwait(false);

            return new LatestBlockReply
            {
                Block = new Block
                {
                    Hash = BitConverter.ToString(latestBlock.Hash),
                    BlockNumber = latestBlock.BlockNumber,
                    EpochNumber = latestBlock.EpochNumber,
                    EpochSlotNumber = latestBlock.EpochSlotNumber,
                    SlotNumber = latestBlock.SlotNumber,
                    Size = latestBlock.Size,
                    SlotLeader = latestBlock.SlotLeaderId,
                    PreviousID = latestBlock.PreviousID,
                    Timestamp = latestBlock.Timestamp.ToTimestamp()
                }
            };
        }

        public override async Task BlockUpdates(BlockUpdatesRequest request, IServerStreamWriter<Block> responseStream, ServerCallContext context)
        {
            Database.Model.Block lastBlockSent = null;
            while (context.CancellationToken.IsCancellationRequested == false)
            {
                var latestBlock = await this._databaseProxy.GetLatestBlock().ConfigureAwait(false);
                if (lastBlockSent == null || lastBlockSent.BlockNumber < latestBlock.BlockNumber)
                {
                    await responseStream.WriteAsync(new Block
                    {
                        Hash = BitConverter.ToString(latestBlock.Hash),
                        BlockNumber = latestBlock.BlockNumber,
                        EpochNumber = latestBlock.EpochNumber,
                        EpochSlotNumber = latestBlock.EpochSlotNumber,
                        SlotNumber = latestBlock.SlotNumber,
                        Size = latestBlock.Size,
                        SlotLeader = latestBlock.SlotLeaderId,
                        PreviousID = latestBlock.PreviousID,
                        Timestamp = latestBlock.Timestamp.ToTimestamp()
                    });
                }

                await Task.Delay((int) request.DelayUpdatesMillisecond);
            }
        }
    }
}
