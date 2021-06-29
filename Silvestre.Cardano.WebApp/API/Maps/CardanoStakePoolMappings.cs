using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.API.ServiceModel.StakePool;

namespace Silvestre.Cardano.WebApp.API
{
    public static class CardanoStakePoolMappings
    {
        public static StakePool ToStakePool(this CardanoStakePool stakePool)
        {
            return new StakePool
            {
                Ticker = stakePool.Ticker,
                Name = stakePool.Name,
                Description = stakePool.Description,
                Website = stakePool.Website,
                MetadataUrl = stakePool.MetadataUrl,

                PoolAddress = stakePool.PoolAddress.Address,
                MarginPercentage = stakePool.Margin.Quantity,
                MaintenanceInADA = stakePool.Maintenance.Quantity,
                PledgeInADA = stakePool.Pledge.Quantity,
                DelegationInADA = stakePool.Delegation.Quantity
            };
        }

        public static StakePoolMetadata ToStakePoolMetadata(this CardanoStakePoolMetadata metadata)
        {
            return new StakePoolMetadata
            {
                Ticker = metadata.Ticker,
                Name = metadata.Name,
                Description = metadata.Description,
                Website = metadata.Website
            };
        }
    }
}
