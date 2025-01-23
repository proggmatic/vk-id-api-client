using System.Text.Json;
using System.Text.Json.Serialization;


namespace Proggmatic.VkIDApiClient;

internal class DateOnlySafeJsonConverter : JsonConverter<DateOnly?>
{
    private static readonly string[] _dateFormats = ["dd.MM.yyyy", "yyyy-MM-dd"];


    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (DateOnly.TryParseExact(value, _dateFormats, out DateOnly dateOnly))
            return dateOnly;

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString("dd.MM.yyyy"));
    }
}