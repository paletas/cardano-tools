using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Silvestre.Cardano.WebApp.Pages.Transactions
{
    public class ViewerModel : PageModel
    {
        public string TransactionId { get; private set; }

        public void OnGet(string id)
        {
            this.TransactionId = id;

            this.ViewData["Subtitle"] = id;
        }
    }
}
