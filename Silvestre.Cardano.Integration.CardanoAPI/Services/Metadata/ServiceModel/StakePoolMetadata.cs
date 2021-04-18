﻿using System;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.Metadata.ServiceModel
{
    internal class StakePoolMetadata
    {
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("homepage")]
        public Uri Website { get; set; }
    }
}
