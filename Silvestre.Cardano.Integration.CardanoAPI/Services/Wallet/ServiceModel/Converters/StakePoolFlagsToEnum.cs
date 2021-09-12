using System.Text.Json;
using System.Text.Json.Serialization;

namespace Silvestre.Cardano.Integration.CardanoAPI.Services.CardanoWalletAPI.ServiceModel.Converters
{
    internal class StakePoolFlagsToEnum : JsonConverter<StakePoolsFlagEnum>
    {
        public override StakePoolsFlagEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert == typeof(IEnumerable<string>))
            {
                if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException();

                var entries = new List<string>();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray) break;
                    else if (reader.TokenType == JsonTokenType.String)
                    {
                        var value = reader.GetString();

                        entries.Add(value);
                    }
                }

                return EnumerableToFlags(entries);
            }
            else throw new NotSupportedException();
        }

        private StakePoolsFlagEnum EnumerableToFlags(List<string> entries)
        {
            StakePoolsFlagEnum flags = StakePoolsFlagEnum.Empty;

            foreach (var flag in entries)
            {
                switch (flag.ToLowerInvariant())
                {
                    case "delisted": flags |= StakePoolsFlagEnum.Delisted; break;
                    default: throw new JsonException($"Unknown flag '{flag}'");
                }
            }

            return flags;
        }

        public override void Write(Utf8JsonWriter writer, StakePoolsFlagEnum value, JsonSerializerOptions options)
        {
            var flags = FlagsToEnumerable(value);

            writer.WriteStartArray();

            foreach (var flag in flags)
            {
                writer.WriteStringValue(flag);
            }

            writer.WriteEndArray();
        }

        private IEnumerable<string> FlagsToEnumerable(StakePoolsFlagEnum value)
        {
            if (value.HasFlag(StakePoolsFlagEnum.Delisted))
                yield return "delisted";
        }
    }
}
