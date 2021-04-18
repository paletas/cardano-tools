using Microsoft.AspNetCore.Mvc;
using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.API.ServiceModel.StakePools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Silvestre.Cardano.WebApp.API
{
    [Route("api/v1/stakepool")]
    [ApiController]
    public class StakePoolsController : ControllerBase
    {
        private readonly CardanoAPI _cardanoAPI;

        public StakePoolsController(CardanoAPI cardanoAPI)
        {
            this._cardanoAPI = cardanoAPI;
        }

        [HttpGet]
        public async IAsyncEnumerable<StakePool> Get([FromQuery(Name = "count")] uint count, [FromQuery(Name = "offset")] uint offset = 0)
        {
            var foundStakePools = this._cardanoAPI.ListStakePools(count, offset);

            await foreach (var stakePool in foundStakePools.ConfigureAwait(false))
            {
                yield return new StakePool
                {
                    Ticker = stakePool.Ticker,
                    Name = stakePool.Name,
                    Description = stakePool.Description,
                    Website = stakePool.Website,

                    PoolAddress = stakePool.PoolAddress.Address,
                    RewardsAddress = stakePool.RewardsAddress.Address,
                    MarginPercentage = stakePool.Margin.Quantity,
                    MaintenanceInADA = stakePool.Maintenance.Quantity,
                    PledgeInADA = stakePool.Pledge.Quantity
                };
            }
        }

        [HttpGet("{address}")]
        public async Task<StakePool> Get([FromRoute(Name = "address")] string address)
        {
            var stakePool = await this._cardanoAPI.GetStakePool(address).ConfigureAwait(false);
            if (stakePool == null) return null;

            return new StakePool
            {
                Ticker = stakePool.Ticker,
                Name = stakePool.Name,
                Description = stakePool.Description,
                Website = stakePool.Website,

                PoolAddress = stakePool.PoolAddress.Address,
                RewardsAddress = stakePool.RewardsAddress.Address,
                MarginPercentage = stakePool.Margin.Quantity,
                MaintenanceInADA = stakePool.Maintenance.Quantity,
                PledgeInADA = stakePool.Pledge.Quantity
            };
        }
    }
}
