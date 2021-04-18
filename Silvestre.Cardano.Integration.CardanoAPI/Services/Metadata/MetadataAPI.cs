using Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI.ServiceModel.StakePools;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.Metadata
{
    internal class MetadataAPI
    {
        public async Task<StakePoolMetadata?> GetMetadata(Uri metadataUrl)
        {
            var httpClient = new HttpClient();
            try
            {
                var metadataResponse = await httpClient.GetAsync(metadataUrl).ConfigureAwait(false);


                if (metadataResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using var metadataStream = await metadataResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    return await JsonSerializer.DeserializeAsync<StakePoolMetadata>(metadataStream).ConfigureAwait(false);
                }
                else
                {
                    return null;
                }
            }
            catch (JsonException)
            {
                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}
