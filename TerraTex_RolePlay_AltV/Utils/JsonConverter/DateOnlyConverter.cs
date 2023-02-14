using System.Text.Json;
using System.Text.Json.Serialization;

namespace TerraTex_RolePlay_AltV_Server.Utils.JsonConverter;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    public override DateOnly Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        DateOnly.Parse(reader.GetString()!);

    public override void Write(
        Utf8JsonWriter writer,
        DateOnly dateTimeValue,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(dateTimeValue.ToString());
    
}