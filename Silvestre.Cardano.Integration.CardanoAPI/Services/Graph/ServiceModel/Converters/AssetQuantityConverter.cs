using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.GraphAPI.ServiceModel.Converters
{
    internal class AssetQuantityConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return long.Parse(reader.GetString()) / 1000000m;
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value * 1000000);
        }
    }
}
