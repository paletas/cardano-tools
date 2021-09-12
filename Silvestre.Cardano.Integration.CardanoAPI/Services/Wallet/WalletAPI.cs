using Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI.ServiceModel.StakePools.Model;
using System.Net.Http.Json;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI
{
    internal class WalletAPI
    {
        private string _baseAddress;

        public WalletAPI(string baseAddress)
        {
            this._baseAddress = baseAddress;
        }

        public async Task<IEnumerable<StakePool>> GetStakePools(long delegationIntent = 0)
        {
            using var httpClient = new HttpClient();
            var endpoint = Path.Combine(_baseAddress, "stake-pools");
            endpoint = $"{endpoint}?stake={delegationIntent}";

            var responseJSON = await httpClient.GetAsync(endpoint).ConfigureAwait(false);
            return await responseJSON.Content.ReadFromJsonAsync<ListStakePoolsResponse>().ConfigureAwait(false);
        }
    }
}
