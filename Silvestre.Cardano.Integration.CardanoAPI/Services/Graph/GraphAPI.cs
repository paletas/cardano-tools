using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI.ServiceModel.StakePools;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI
{
    internal class GraphAPI
    {
        private Uri _baseAddress;
        private GraphQLHttpClient _graphQL;
        private HttpClient _httpClient;

        public GraphAPI(Uri baseAddress)
        {
            this._baseAddress = baseAddress;
            this._httpClient = new HttpClient(new SocketsHttpHandler { ConnectTimeout = TimeSpan.FromSeconds(5) });
            this._graphQL = new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = _baseAddress }, new GraphQL.Client.Serializer.SystemTextJson.SystemTextJsonSerializer(), _httpClient);
        }

        public async Task<IEnumerable<StakePool>> ListStakePools(int count = 10, int offset = 0)
        {
            const string query = @"query ListStakePools($limit: Int!, $offset: Int!) {
              stakePools(limit: $limit, offset: $offset) {
                id
                url
                fixedCost
                margin
                pledge
                rewardAddress
                owners {
                  hash
                }
                activeStake_aggregate {
                  aggregate { sum { amount } }
                }    
              }
            }";

            var response = await this._graphQL.SendQueryAsync<ListStakePoolsResponse>(query, new { limit = count, offset = offset }).ConfigureAwait(false);

            return response.Data.StakePools;
        }
    }
}
