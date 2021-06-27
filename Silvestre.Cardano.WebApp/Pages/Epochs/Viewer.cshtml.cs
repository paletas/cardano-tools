using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Silvestre.Cardano.Integration.CardanoAPI;
using System.Threading.Tasks;

namespace Silvestre.Cardano.WebApp.Pages.Epochs
{
    public class ViewerModel : PageModel
    {
        private CardanoAPI CardanoAPI { get; set; }

        public ViewerModel(CardanoAPI cardanoAPI)
        {
            this.CardanoAPI = cardanoAPI;
        }

        [BindProperty(SupportsGet = true)]
        public ulong EpochNumber { get; set; }

        public async Task OnGet(ulong? epochNumber = null)
        {
            var currentEpoch = await CardanoAPI.GetCurrentEpoch();
            this.EpochNumber = epochNumber ?? currentEpoch.Number;
        }
    }
}
