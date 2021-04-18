using Silvestre.Cardano.WebApp.SignalR.ServiceModel;
using System.Threading.Tasks;

namespace Silvestre.Cardano.WebApp.SignalR
{
    public interface IBlocksHubClient
    {
        Task NewBlock(Block block);

        Task NewEpoch(uint epochNumber);
    }
}
