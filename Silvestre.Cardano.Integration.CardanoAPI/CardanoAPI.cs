using Silvestre.Cardano.Integration.CardanoAPI.Services.DbSync;
using Silvestre.Cardano.Integration.CardanoAPI.Services.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.CardanoAPI
{
    public class CardanoAPI
    {
        private MetadataAPI _metadataAPI;
        private DbSyncAPI _dbSyncAPI;

        public CardanoAPI(string dbSyncEndpoint) : this(new Uri(dbSyncEndpoint))
        { }

        public CardanoAPI(Uri dbSyncEndpoint)
        {
            _metadataAPI = new MetadataAPI();
            _dbSyncAPI = new DbSyncAPI(dbSyncEndpoint);
        }

        public async Task<CurrentCardanoEpoch> GetCurrentEpoch()
        {
            var epoch = await _dbSyncAPI.GetCurrentEpoch().ConfigureAwait(false);
            var latestBlock = await _dbSyncAPI.GetLatestBlock(epoch.Number).ConfigureAwait(false);

            return new CurrentCardanoEpoch(
                CardanoEra.MaryEra,
                epoch.Number,
                epoch.TransactionCount,
                epoch.BlockCount,
                new CardanoAsset(epoch.OutSum.ToDecimal(CardanoAsset.ADA_DECIMALPOINTER), CardanoAsset.ADA_UNIT),
                new CardanoAsset(epoch.Fees, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                epoch.StartTime.ToDateTime(),
                epoch.EndTime.ToDateTime(),
                latestBlock.EpochSlotNumber
            );
        }

        public async Task<CardanoEpoch> GetEpoch(uint epochNumber)
        {
            var epoch = await _dbSyncAPI.GetEpoch(epochNumber).ConfigureAwait(false);

            return new CardanoEpoch(
                CardanoEra.MaryEra,
                epoch.Number,
                epoch.TransactionCount,
                epoch.BlockCount,
                new CardanoAsset(epoch.OutSum.ToDecimal(CardanoAsset.ADA_DECIMALPOINTER), CardanoAsset.ADA_UNIT),
                new CardanoAsset(epoch.Fees, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                epoch.StartTime.ToDateTime(),
                epoch.EndTime.ToDateTime()
            );
        }

        public IAsyncEnumerable<CardanoBlock> BlockUpdates(uint delayMilliseconds, CancellationToken cancellationToken)
        {
            return _dbSyncAPI.BlockUpdates(delayMilliseconds, cancellationToken).Select(block => new CardanoBlock
            {
                Hash = block.Hash,
                BlockNumber = block.BlockNumber,
                EpochNumber = block.EpochNumber,
                EpochSlotNumber = block.EpochSlotNumber,
                SlotNumber = block.SlotNumber,
                SlotLeader = block.SlotLeader,
                Size = block.Size,
                PreviousID = block.PreviousID,
                Timestamp = block.Timestamp.ToDateTime()
            });
        }

        public async Task<(ulong Total, ulong From, IEnumerable<CardanoStakePool> StakePools)> ListStakePools(uint? epochNumber, uint count, uint offset = 0, bool includeMetadata = false)
        {
            var stakePoolsReply = await _dbSyncAPI.ListStakePools(epochNumber, offset, count);
            var stakePools = new List<CardanoStakePool>(stakePoolsReply.StakePools.Count);

            foreach (var stakePool in stakePoolsReply.StakePools)
            {
                stakePools.Add(new CardanoStakePool
                {
                    MetadataUrl = new Uri(stakePool.MetadataUrl),
                    PoolAddress = new CardanoAddress(stakePool.PoolAddress, CardanoAddress.AddressKindEnum.StakePool),
                    Margin = new CardanoAsset((decimal)stakePool.Margin, CardanoAsset.PERCENTAGE_UNIT),
                    Maintenance = new CardanoAsset(stakePool.FixedCost, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                    Pledge = new CardanoAsset(stakePool.Pledge, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                    Delegation = new CardanoAsset(stakePool.Delegation, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT)
                });
            }

            if (includeMetadata)
            {
                Parallel.ForEach(stakePools, async stakePool =>
                {
                    var metadata = await _metadataAPI.GetMetadata(stakePool.MetadataUrl);

                    stakePool.Ticker = metadata?.Ticker;
                    stakePool.Name = metadata?.Name;
                    stakePool.Description = metadata?.Description;
                    stakePool.Website = metadata?.Homepage;
                });
            }

            return (stakePoolsReply.Total, stakePoolsReply.From, stakePools);
        }

        public async Task<CardanoStakePool?> GetStakePool(string poolAddress)
        {
            var stakePool = await _dbSyncAPI.GetStakePool(poolAddress).ConfigureAwait(false);
            if (stakePool == null) return null;

            var metadata = await _metadataAPI.GetMetadata(new Uri(stakePool.MetadataUrl)).ConfigureAwait(false);
            return new CardanoStakePool
            {
                Ticker = metadata?.Ticker,
                Name = metadata?.Name,
                Description = metadata?.Description,
                Website = metadata?.Homepage,
                MetadataUrl = new Uri(stakePool.MetadataUrl),

                PoolAddress = new CardanoAddress(stakePool.PoolAddress, CardanoAddress.AddressKindEnum.StakePool),
                Margin = new CardanoAsset((decimal)stakePool.Margin, CardanoAsset.PERCENTAGE_UNIT),
                Maintenance = new CardanoAsset(stakePool.FixedCost, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                Pledge = new CardanoAsset(stakePool.Pledge, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                Delegation = new CardanoAsset(stakePool.Delegation, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT)
            };
        }

        public async Task<CardanoStakePoolMetadata> GetStakePoolMetadata(Uri metadataUrl)
        {
            var metadata = await _metadataAPI.GetMetadata(metadataUrl).ConfigureAwait(false);

            return new CardanoStakePoolMetadata
            {
                Ticker = metadata?.Ticker,
                Name = metadata?.Name,
                Description = metadata?.Description,
                Website = metadata?.Homepage
            };
        }

        public async Task<CardanoTransaction?> GetTransactionOutput(string transactionAddress)
        {
            var transaction = await _dbSyncAPI.GetTransaction(transactionAddress).ConfigureAwait(false);
            if (transaction == null) return null;

            return new CardanoTransaction
            {
                TransactionId = transaction.TransactionId,
                TransactionHash = transaction.TransactionHash,
                TransactionCount = transaction.TransactionCount,
                TransactionSize = transaction.TransactionSize,
                TransactionOutputTotal = new CardanoAsset(transaction.OutSum, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                BlockNumber = transaction.BlockNumber,
                BlockSize = transaction.BlockSize,
                EpochNumber = transaction.EpochNumber,
                EpochSlotNumber = transaction.EpochSlotNumber,
                SlotNumber = transaction.SlotNumber,
                Deposit = new CardanoAsset(transaction.Deposit, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                Fees = new CardanoAsset(transaction.Fees, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                InvalidAfterBlock = transaction.InvalidAfterBlock,
                InvalidBeforeBlock = transaction.InvalidBeforeBlock,
                Timestamp = transaction.Timestamp.ToDateTime(),
                TransactionInId = transaction.TransactionInId,
                Metadata = transaction.Metadata.Select(metadata => new CardanoTransactionMetadata { MetadataKey = metadata.Key, MetadataJson = metadata.Json }).ToArray(),
                Output = transaction.Output.Select(output => new CardanoTransactionOutput { AddressTo = output.Address, Output = new CardanoAsset(output.Amount, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT) }).ToArray()
            };
        }

        public async Task<IEnumerable<CardanoBlockDetail>> GetEpochBlocks(uint epochNumber)
        {
            var blocks = await _dbSyncAPI.GetBlocks(epochNumber).ConfigureAwait(false);

            return blocks.Select(b =>
                new CardanoBlockDetail
                {
                    Hash = b.Block.Hash,
                    EpochNumber = b.Block.EpochNumber,
                    EpochSlotNumber = b.Block.EpochSlotNumber,
                    SlotNumber = b.Block.SlotNumber,
                    BlockNumber = b.Block.BlockNumber,
                    PreviousID = b.Block.PreviousID,
                    Size = b.Block.Size,
                    SlotLeader = b.Block.SlotLeader,
                    AmountTransacted = new CardanoAsset(b.TotalOutSum.ToDecimal(CardanoAsset.ADA_DECIMALPOINTER), CardanoAsset.ADA_UNIT),
                    Fees = new CardanoAsset(b.TotalFees, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                    Timestamp = b.Block.Timestamp.ToDateTime()
                }
            ).ToArray();
        }

        public async Task<CardanoEpochStakePoolStatistics> GetEpochDelegationStatistics(uint epochNumber)
        {
            return await _dbSyncAPI.GetEpochDelegationStatistics(epochNumber).ConfigureAwait(false);
        }

        public async Task<CardanoEpochSupplyStatistics> GetEpochSupplyStatistics(uint epochNumber)
        {
            return await _dbSyncAPI.GetEpochSupplyStatistics(epochNumber).ConfigureAwait(false);
        }
    }
}
