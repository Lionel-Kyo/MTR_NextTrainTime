using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTR_NextTrain.Api
{
    internal class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        private string serializeFormat;
        public DateTimeJsonConverter(string serializeFormat = null)
        {
            this.serializeFormat = serializeFormat;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString() ?? string.Empty);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (string.IsNullOrEmpty(serializeFormat))
                writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
            else
                writer.WriteStringValue(value.ToString(this.serializeFormat));
        }
    }
}
