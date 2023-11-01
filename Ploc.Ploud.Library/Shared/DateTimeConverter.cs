using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            double doubleValue = reader.GetDouble();
            return ((long)doubleValue).DateTimeValue();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.GetSecondsSince1970());
        }
    }
}
