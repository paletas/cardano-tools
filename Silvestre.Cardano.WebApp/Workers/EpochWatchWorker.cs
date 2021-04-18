using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Silvestre.Cardano.Integration.CardanoAPI;
using Silvestre.Cardano.WebApp.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Silvestre.Cardano.WebApp.Workers
{
    public class EpochWatchWorker : BackgroundService
    {
        private readonly CardanoAPI _cardanoAPI;
        private readonly IHubContext<BlocksHub, IBlocksHubClient> _blocksHub;

        public EpochWatchWorker(CardanoAPI cardanoAPI, IHubContext<BlocksHub, IBlocksHubClient> blocksHub)
        {
            this._cardanoAPI = cardanoAPI;
            this._blocksHub = blocksHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var blockStreaming = _cardanoAPI.BlockUpdates(1000, stoppingToken);

            uint currentEpoch = 0;

            await foreach (var block in blockStreaming)
            {
                if (block.EpochNumber != currentEpoch)
                {
                    currentEpoch = block.EpochNumber;
                    await _blocksHub.Clients.All.NewEpoch(currentEpoch);
                }

                await _blocksHub.Clients.All.NewBlock(new SignalR.ServiceModel.Block
                {
                    BlockNumber = block.BlockNumber,
                    EpochNumber = block.EpochNumber,
                    EpochSlotNumber = block.EpochSlotNumber,
                    SlotNumber = block.SlotNumber,
                    Hash = block.Hash,
                    Size = block.Size,
                    Timestamp = block.Timestamp
                });
            }
        }
    }
}
