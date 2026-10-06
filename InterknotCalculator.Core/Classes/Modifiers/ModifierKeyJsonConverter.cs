using System.Text.Json;
using System.Text.Json.Serialization;

namespace InterknotCalculator.Core.Classes.Modifiers;

public class ModifierKeyJsonConverter : JsonConverter<ModifierKey> {
    public override ModifierKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var raw = reader.GetString() ?? "";
        return new(raw.Split(";"));
    }

    public override void Write(Utf8JsonWriter writer, ModifierKey value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}