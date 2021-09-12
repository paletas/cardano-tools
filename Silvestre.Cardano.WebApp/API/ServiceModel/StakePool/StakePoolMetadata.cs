using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.StakePool
{
    [DebuggerDisplay("{Ticker}")]
    public class StakePoolMetadata
    {
        [JsonPropertyName("ticker")]
        public string Ticker { get; internal set; }

        [JsonPropertyName("name")]
        public string Name { get; internal set; }

        [JsonPropertyName("description")]
        public string Description { get; internal set; }

        [JsonPropertyName("websiteUrl")]
        public Uri Website { get; internal set; }
    }
}
