using Silvestre.Cardano.Integration.CardanoAPI.Services.DbSync;
using Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI;
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
        private GraphAPI _graphAPI;
        private DbSyncAPI _dbSyncAPI;

        public CardanoAPI(string graphEndpoint, string dbSyncEndpoint) : this(new Uri(graphEndpoint), new Uri(dbSyncEndpoint))
        { }

        public CardanoAPI(Uri graphEndpoint, Uri dbSyncEndpoint)
        {
            _metadataAPI = new MetadataAPI();
            _graphAPI = new GraphAPI(graphEndpoint);
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
                epoch.OutSum.ToInt128(),
                new CardanoAsset(epoch.Fees, CardanoAsset.ADA_DECIMALPOINTER, CardanoAsset.ADA_UNIT),
                epoch.StartTime.ToDateTime(),
                epoch.EndTime.ToDateTime(),
                latestBlock.EpochSlotNumber
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

        public async IAsyncEnumerable<CardanoStakePool> ListStakePools(uint count, uint offset = 0)
        {
            var foundStakePools = await _dbSyncAPI.ListStakePools(offset, count);

            foreach (var stakePool in foundStakePools)
            {
                var metadata = await _metadataAPI.GetMetadata(new Uri(stakePool.MetadataUrl)).ConfigureAwait(false);

                yield return new CardanoStakePool
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
                    RewardsAddress = new CardanoAddress(stakePool.RewardAddress, CardanoAddress.AddressKindEnum.Rewards)
                };
            }
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
                RewardsAddress = new CardanoAddress(stakePool.RewardAddress, CardanoAddress.AddressKindEnum.Rewards)
            };
        }
    }
}
