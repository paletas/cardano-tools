using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Silvestre.Cardano.WebApp.Pages.StakePools
{
    public class IndexModel : PageModel
    {
        private IConfiguration Configuration { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public string FeaturedStakePool { get; set; }

        public void OnGet()
        {
            this.ViewData["Subtitle"] = "Stake Pools";
            this.FeaturedStakePool = Configuration.GetSection("Features")?["FeaturedStakePool"];
        }
    }
}
