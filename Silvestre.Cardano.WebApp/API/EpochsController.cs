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
        public async Task<Epoch> GetEpoch([FromRoute] uint epochNumber)
        {
            var epoch = await this._cardanoAPI.GetEpoch(epochNumber).ConfigureAwait(false);

            return new Epoch
            {
                Number = epoch.Number,
                StartTime = epoch.StartTime,
                EndTime = epoch.EndTime,
                TransactionsCount = epoch.TransactionsCount,
                BlocksCount = epoch.BlockCount,
                MaximumSlots = epoch.MaxSlots,
                TransactionsTotal = epoch.TransactionsTotal.Quantity,
                FeesTotal = epoch.Fees.Quantity
            };
        }

        [HttpGet("{epochNumber}/statistics")]
        public async Task<EpochStatistics> GetEpochStatistics([FromRoute] uint epochNumber)
        {
            var epochStatistics = await this._cardanoAPI.GetEpochStatistics(epochNumber).ConfigureAwait(false);

            return new EpochStatistics
            {
                EpochNumber = epochStatistics.EpochNumber,
                TotalStakePools = epochStatistics.TotalStakePools,
                TotalDelegations = epochStatistics.TotalDelegations,
                CirculatingSupply = epochStatistics.CirculatingSupply.Quantity,
                StakedSupply = epochStatistics.DelegatedSupply.Quantity,
                Rewards = epochStatistics.Rewards?.Quantity,
                OrphanedRewards = epochStatistics.OrphanedRewards?.Quantity
            };
        }
    }
}
