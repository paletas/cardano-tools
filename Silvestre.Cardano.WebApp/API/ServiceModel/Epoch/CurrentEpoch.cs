using System.Text.Json.Serialization;

namespace Silvestre.Cardano.WebApp.API.ServiceModel.Epoch
{
    public class CurrentEpoch : Epoch
    {
        [JsonPropertyName("currentSlotNumber")]
        public uint CurrentSlotNumber { get; set; }
    }
}
