using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.Block
{
    public class BlockSummarized : Block
    {
        [JsonPropertyName("amountTransacted")]
        public decimal AmountTransacted { get; set; }

        [JsonPropertyName("fees")]
        public decimal Fees { get; set; }
    }
}
