using Microsoft.AspNetCore.Mvc;
using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.API.ServiceModel.Block;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Silvestre.Cardano.WebApp.API
{
    [Route("api/v1/")]
    [ApiController]
    public class BlocksController : ControllerBase
    {
        private readonly CardanoAPI _cardanoAPI;

        public BlocksController(CardanoAPI cardanoAPI)
        {
            this._cardanoAPI = cardanoAPI;
        }

        [HttpGet("epoch/{epochNumber}/block")]
        public async Task<IEnumerable<BlockSummarized>> GetEpochBlocks([FromRoute] uint epochNumber)
        {
            var blocks = await this._cardanoAPI.GetEpochBlocks(epochNumber).ConfigureAwait(false);

            return blocks.Select(block => new BlockSummarized
            {
                EpochNumber = block.EpochNumber,
                EpochSlotNumber = block.EpochSlotNumber,
                BlockNumber = block.BlockNumber,
                Hash = block.Hash,
                SlotLeader = block.SlotLeader,
                SlotNumber = block.SlotNumber,
                PreviousID = block.PreviousID,
                Size = block.Size,
                Timestamp = block.Timestamp,
                AmountTransacted = block.AmountTransacted.Quantity,
                Fees = block.Fees.Quantity
            });
        }
    }
}
