using Microsoft.AspNetCore.Mvc;
using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.API.ServiceModel.StakePool;
using System;
using System.Linq;
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
        public async Task<ListStakePoolsResponse> List([FromQuery(Name = "epoch")] uint? epochNumber, [FromQuery(Name = "count")] uint count, [FromQuery(Name = "offset")] uint offset = 0, [FromQuery(Name = "includeMetadata")] bool includeMetadata = false)
        {
            var (total, from, stakePools) = await this._cardanoAPI.ListStakePools(epochNumber, count, offset, includeMetadata);

            return new ListStakePoolsResponse
            {
                Total = total,
                From = from,
                StakePools = stakePools.Select(CardanoStakePoolMappings.ToStakePool).ToArray()
            };
        }

        [HttpGet("{address}")]
        public async Task<StakePool> Get([FromRoute(Name = "address")] string address)
        {
            var stakePool = await this._cardanoAPI.GetStakePool(address).ConfigureAwait(false);
            if (stakePool == null) return null;

            return stakePool.ToStakePool();
        }

        [HttpGet("metadata")]
        public async Task<StakePoolMetadata> GetMetadata([FromQuery(Name = "url")] string metadataUrl)
        {
            var stakePoolMetadata = await this._cardanoAPI.GetStakePoolMetadata(new Uri(metadataUrl, UriKind.Absolute));

            return stakePoolMetadata.ToStakePoolMetadata();
        }
    }
}
