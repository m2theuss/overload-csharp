using System.Text.Json;
using System.Text.Json.Serialization;
using overload.Exceptions;

namespace overload.Common.Converters;

public class SafeDateOnlyConverter : JsonConverter<DateOnly>
{
     private readonly string _format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        
        if (DateOnly.TryParseExact(value, _format, out var date))
        {
            return date;
        }

        // em vez de deixar o JSON quebrar com um erro genérico, 
        // no caso pode lançar uma exceção customizada que sua API capture
        throw new JsonParserException($"Invalid birth date, it must be like: {_format}.");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}