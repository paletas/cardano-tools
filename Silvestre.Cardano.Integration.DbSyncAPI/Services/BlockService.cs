using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Silvestre.Cardano.Integration.DbSync.Services;

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
            var latestBlock = await this._databaseProxy.GetLatestBlock(context.CancellationToken, request.EpochNumber).ConfigureAwait(false);

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
                var latestBlock = await this._databaseProxy.GetLatestBlock(context.CancellationToken).ConfigureAwait(false);
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
                        Timestamp = latestBlock.Timestamp.ToTimestamp(),
                        TransactionCount = latestBlock.TransactionCount
                    });
                }

                await Task.Delay((int)request.DelayUpdatesMillisecond);
            }
        }

        public override async Task<GetBlocksReply> GetBlocks(GetBlocksRequest request, ServerCallContext context)
        {
            var epochBlocks = await this._databaseProxy.GetEpochBlocks(context.CancellationToken, request.EpochNumber).ConfigureAwait(false);

            var reply = new GetBlocksReply();
            reply.Blocks.AddRange(epochBlocks.Select(b => new BlockDetail
            {
                Block = new Block
                {
                    Hash = BitConverter.ToString(b.Hash),
                    BlockNumber = b.BlockNumber,
                    EpochNumber = b.EpochNumber,
                    EpochSlotNumber = b.EpochSlotNumber,
                    SlotNumber = b.SlotNumber,
                    Size = b.Size,
                    SlotLeader = b.SlotLeaderId,
                    PreviousID = b.PreviousID,
                    Timestamp = b.Timestamp.ToTimestamp(),
                    TransactionCount = b.TransactionCount
                },
                TotalFees = b.TotalFees,
                TotalOutSum = b.TotalOutSum.ToInt128()
            }));

            return reply;
        }
    }
}
