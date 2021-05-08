using Grpc.Core;
using Grpc.Net.Client;
using Silvestre.Cardano.Integration.DbSync.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.DbSync
{
    internal class DbSyncAPI
    {
        private readonly Uri _baseAddress;
        private readonly Epochs.EpochsClient _epochsClient;
        private readonly Blocks.BlocksClient _blocksClient;
        private readonly StakePools.StakePoolsClient _stakePoolsClient;

        public DbSyncAPI(string baseAddress) : this(new Uri(baseAddress))
        { }

        public DbSyncAPI(Uri baseAddress)
        {
            this._baseAddress = baseAddress;

            this._epochsClient = new Epochs.EpochsClient(GrpcChannel.ForAddress(baseAddress));
            this._blocksClient = new Blocks.BlocksClient(GrpcChannel.ForAddress(baseAddress));
            this._stakePoolsClient = new StakePools.StakePoolsClient(GrpcChannel.ForAddress(baseAddress));
        }


        public async Task<Epoch> GetCurrentEpoch()
        {
            var response = await this._epochsClient.GetCurrentEpochAsync(new CurrentEpochRequest()).ResponseAsync.ConfigureAwait(false);
            return response.Epoch;
        }

        public async Task<Block> GetLatestBlock(uint epochNumber)
        {
            var response = await this._blocksClient.GetLatestBlockAsync(new LatestBlockRequest { EpochNumber = epochNumber }).ResponseAsync.ConfigureAwait(false);
            return response.Block;
        }

        public IAsyncEnumerable<Block> BlockUpdates(uint delayMilliseconds, CancellationToken cancellationToken)
        {
            var streamingResponse = _blocksClient.BlockUpdates(new BlockUpdatesRequest { DelayUpdatesMillisecond = delayMilliseconds }, cancellationToken: cancellationToken);
            return streamingResponse.ResponseStream.ReadAllAsync(cancellationToken);
        }

        public async Task<ListStakePoolsReply> ListStakePools(uint offset = 0, uint limit = 100)
        {
            return await this._stakePoolsClient.ListStakePoolsAsync(new ListStakePoolsRequest { Offset = offset, Limit = limit }).ResponseAsync.ConfigureAwait(false);
        }

        public async Task<StakePool> GetStakePool(string poolAddress)
        {
            var response = await this._stakePoolsClient.GetStakePoolAsync(new GetStakePoolRequest { PoolAddress = poolAddress }).ResponseAsync.ConfigureAwait(false);
            return response.StakePool;
        }
    }
}
