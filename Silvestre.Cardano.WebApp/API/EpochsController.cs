using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.API.ServiceModel.Epoch;
using System.Threading.Tasks;

namespace Silvestre.Cardano.WebApp.API
{
    [Route("api/v1/epoch")]
    [ApiController]
    public class EpochsController : ControllerBase
    {
        private readonly CardanoAPI _cardanoAPI;

        public EpochsController(CardanoAPI cardanoAPI)
        {
            this._cardanoAPI = cardanoAPI;
        }


        [HttpGet]
        public async Task<CurrentEpoch> GetCurrentEpoch()
        {
            var currentEpoch = await this._cardanoAPI.GetCurrentEpoch().ConfigureAwait(false);

            return new CurrentEpoch
            {
                Number = currentEpoch.Number,
                StartTime = currentEpoch.StartTime,
                EndTime = currentEpoch.EndTime,
                TransactionsCount = currentEpoch.TransactionsCount,
                BlocksCount = currentEpoch.BlockCount,
                CurrentSlotNumber = currentEpoch.CurrentSlot,
                MaximumSlots = currentEpoch.MaxSlots,
                TransactionsTotal = currentEpoch.TransactionsTotal.Quantity,
                FeesTotal = currentEpoch.Fees.Quantity
            };
        }

        [HttpGet("{epochNumber}")]
        public async Task<IActionResult> GetEpoch([FromRoute] uint epochNumber)
        {
            try
            {
                var epoch = await this._cardanoAPI.GetEpoch(epochNumber).ConfigureAwait(false);

                return new JsonResult(new Epoch
                {
                    Number = epoch.Number,
                    StartTime = epoch.StartTime,
                    EndTime = epoch.EndTime,
                    TransactionsCount = epoch.TransactionsCount,
                    BlocksCount = epoch.BlockCount,
                    MaximumSlots = epoch.MaxSlots,
                    TransactionsTotal = epoch.TransactionsTotal.Quantity,
                    FeesTotal = epoch.Fees.Quantity
                });
            }
            catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                return new NotFoundResult();
            }
        }

        [HttpGet("{epochNumber}/statistics/delegation")]
        public async Task<EpochDelegationStatistics> GetEpochDelegationStatistics([FromRoute] uint epochNumber)
        {
            var epochStatistics = await this._cardanoAPI.GetEpochDelegationStatistics(epochNumber).ConfigureAwait(false);

            return new EpochDelegationStatistics
            {
                EpochNumber = epochStatistics.EpochNumber,
                TotalStakePools = epochStatistics.TotalStakePools,
                TotalDelegations = epochStatistics.TotalDelegations,
                Rewards = epochStatistics.Rewards?.Quantity,
                OrphanedRewards = epochStatistics.OrphanedRewards?.Quantity
            };
        }

        [HttpGet("{epochNumber}/statistics/supply")]
        public async Task<EpochSupplyStatistics> GetEpochSupplyStatistics([FromRoute] uint epochNumber)
        {
            var epochStatistics = await this._cardanoAPI.GetEpochSupplyStatistics(epochNumber).ConfigureAwait(false);

            return new EpochSupplyStatistics
            {
                EpochNumber = epochStatistics.EpochNumber,
                CirculatingSupply = epochStatistics.CirculatingSupply.Quantity,
                StakedSupply = epochStatistics.StakedSupply.Quantity
            };
        }
    }
}
