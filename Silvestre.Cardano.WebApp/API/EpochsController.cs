using Microsoft.AspNetCore.Mvc;
using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.API.ServiceModel.Epoch;
using System.Threading.Tasks;

namespace Silvestre.Cardano.WebApp.API
{
    [Route("api/v1/epochs")]
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
                TransactionCount = currentEpoch.TransactionCount,
                BlockCount = currentEpoch.BlockCount,
                SlotNumber = currentEpoch.CurrentSlot,
                MaximumSlots = currentEpoch.MaxSlots
            };
        }
    }
}
