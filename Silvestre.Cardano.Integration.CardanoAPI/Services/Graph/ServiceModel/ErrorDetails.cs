using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI.ServiceModel
{
    internal class ErrorDetails
    {
        public class ErrorDetailsExtensions
        {
            [JsonPropertyName("code")]
            public string Code { get; set; }

            [JsonPropertyName("exception.stacktrace")]
            public IEnumerable<string> StackTrace { get; set; }
        }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("path")]
        public IEnumerable<string> Paths { get; set; }
    }
}
