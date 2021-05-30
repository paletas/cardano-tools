using Microsoft.AspNetCore.Mvc;
using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.API.Maps;
using System.Threading.Tasks;

namespace Silvestre.Cardano.WebApp.API
{
    [Route("api/v1/transaction")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly CardanoAPI _cardanoAPI;

        public TransactionsController(CardanoAPI cardanoAPI)
        {
            this._cardanoAPI = cardanoAPI;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute(Name = "id")] string transactionHashId)
        {
            var transaction = await this._cardanoAPI.GetTransactionOutput(transactionHashId).ConfigureAwait(false);
            if (transaction == null) return NotFound();
            else return Ok(transaction.ToTransaction());
        }
    }
}
