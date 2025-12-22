using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MojTaxi.Shared.Converters
{

    public sealed class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        private static readonly string[] Formats =
        {
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd"
    };

        public override DateTime? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            var value = reader.GetString();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParseExact(
                    value,
                    Formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                return date;
            }

            // fallback (ako API nekad pošalje ISO)
            if (DateTime.TryParse(value, out date))
                return date;

            throw new JsonException($"Invalid date format: {value}");
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime? value,
            JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                writer.WriteNullValue();
        }
    }
}
