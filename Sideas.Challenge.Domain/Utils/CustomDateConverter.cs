using Newtonsoft.Json;
using System.Globalization;

namespace Sideas.Challenge.Domain.Utils
{
    // Conversor personalizado para deserializar fechas en formato "dd-MM-yyyy"
    public class CustomDateConverter : JsonConverter
    {
        private static readonly string[] Formats = { "dd-MM-yyyy" };

        // Define si este conversor puede aplicarse a DateTime o DateTime?
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime?) || objectType == typeof(DateTime);
        }

        // Convierte un string a DateTime (lectura)
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (reader.TokenType == JsonToken.String)
            {
                var dateStr = (string)reader.Value!;
                if (DateTime.TryParseExact(dateStr, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                    return date;
            }

            throw new JsonSerializationException($"Invalid date format for property. Expected format: dd-MM-yyyy.");
        }

        // Convierte un DateTime a string (escritura)
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is DateTime date)
                writer.WriteValue(date.ToString("dd-MM-yyyy")); // Escribe fecha como string
            else
                writer.WriteNull(); // Si el valor es nulo, escribe null
        }
    }
}
