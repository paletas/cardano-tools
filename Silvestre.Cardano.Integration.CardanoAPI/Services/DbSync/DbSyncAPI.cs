﻿using Grpc.Core;
using Grpc.Net.Client;
using Silvestre.Cardano.Integration.DbSync.Services;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.DbSync
{
    internal class DbSyncAPI
    {
        private readonly Uri _baseAddress;
        private readonly Epochs.EpochsClient _epochsClient;
        private readonly Blocks.BlocksClient _blocksClient;
        private readonly StakePools.StakePoolsClient _stakePoolsClient;
        private readonly Transactions.TransactionsClient _transactionsClient;

        public DbSyncAPI(string baseAddress) : this(new Uri(baseAddress))
        { }

        public DbSyncAPI(Uri baseAddress)
        {
            this._baseAddress = baseAddress;

            var grpcOptions = new GrpcChannelOptions
            {
                HttpClient = new System.Net.Http.HttpClient { BaseAddress = baseAddress, Timeout = TimeSpan.FromMinutes(5) }
            };

            this._epochsClient = new Epochs.EpochsClient(GrpcChannel.ForAddress(baseAddress, grpcOptions));
            this._blocksClient = new Blocks.BlocksClient(GrpcChannel.ForAddress(baseAddress, grpcOptions));
            this._stakePoolsClient = new StakePools.StakePoolsClient(GrpcChannel.ForAddress(baseAddress, grpcOptions));
            this._transactionsClient = new Transactions.TransactionsClient(GrpcChannel.ForAddress(baseAddress, grpcOptions));
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

        public async Task<ListStakePoolsReply> ListStakePools(uint? epochNumber = null, uint offset = 0, uint limit = 100)
        {
            if (epochNumber == null)
                return await this._stakePoolsClient.ListStakePoolsAsync(new ListStakePoolsRequest { Offset = offset, Limit = limit }).ResponseAsync.ConfigureAwait(false);
            else
                return await this._stakePoolsClient.ListStakePoolsByEpochAsync(new ListStakePoolsByEpochRequest { EpochNumber = epochNumber.Value, Offset = offset, Limit = limit }).ResponseAsync.ConfigureAwait(false);
        }

        public async Task<StakePool> GetStakePool(string poolAddress)
        {
            var response = await this._stakePoolsClient.GetStakePoolAsync(new GetStakePoolRequest { PoolAddress = poolAddress }).ResponseAsync.ConfigureAwait(false);
            return response.StakePool;
        }

        public async Task<Transaction> GetTransaction(string transactionAddress)
        {
            var response = await this._transactionsClient.GetTransactionDetailsAsync(new GetTransactionDetailsRequest { TransactionHashId = transactionAddress }).ResponseAsync.ConfigureAwait(false);
            return response.Transaction;
        }

        public async Task<IEnumerable<BlockDetail>> GetBlocks(uint epochNumber)
        {
            var response = await this._blocksClient.GetBlocksAsync(new GetBlocksRequest { EpochNumber = epochNumber }).ResponseAsync.ConfigureAwait(false);
            return response.Blocks;
        }

        public async Task<Epoch> GetEpoch(uint epochNumber)
        {
            var response = await this._epochsClient.GetEpochAsync(new GetEpochRequest { EpochNumber = epochNumber }).ResponseAsync.ConfigureAwait(false);
            return response.Epoch;
        }

        public async Task<CardanoEpochStakePoolStatistics> GetEpochDelegationStatistics(uint epochNumber)
        {
            var response = await this._epochsClient.GetEpochDelegationStatisticsAsync(new GetEpochDelegationStatisticsRequest { EpochNumber = epochNumber }).ResponseAsync.ConfigureAwait(false);
            return new CardanoEpochStakePoolStatistics
            {
                EpochNumber = response.EpochNumber,
                TotalDelegations = response.TotalDelegations,
                TotalStakePools = response.TotalStakePools,
                Rewards = response.RewardsCalculated ? new CardanoAsset(response.Rewards, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT) : null,
                OrphanedRewards = response.RewardsCalculated ? new CardanoAsset(response.OrphanedRewards, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT) : null
            };
        }

        public async Task<CardanoEpochSupplyStatistics> GetEpochSupplyStatistics(uint epochNumber)
        {
            var response = await this._epochsClient.GetEpochSupplyStatisticsAsync(new GetEpochSupplyStatisticsRequest { EpochNumber = epochNumber }).ResponseAsync.ConfigureAwait(false);
            return new CardanoEpochSupplyStatistics
            {
                EpochNumber = response.EpochNumber,
                CirculatingSupply = new CardanoAsset(response.CirculatingSupply, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                StakedSupply = new CardanoAsset(response.DelegatedSupply, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT)
            };
        }
    }
}
