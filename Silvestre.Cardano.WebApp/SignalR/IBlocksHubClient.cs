using Silvestre.Cardano.WebApp.SignalR.ServiceModel;

namespace Silvestre.Cardano.WebApp.SignalR
{
    public interface IBlocksHubClient
    {
        Task NewBlock(Block block);

        Task NewEpoch(uint epochNumber);
    }
}
