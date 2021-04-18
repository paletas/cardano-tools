using Microsoft.AspNetCore.SignalR;

namespace Silvestre.Cardano.WebApp.SignalR
{
    public class BlocksHub : Hub<IBlocksHubClient>
    {
    }
}
