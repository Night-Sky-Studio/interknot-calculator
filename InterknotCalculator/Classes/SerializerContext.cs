using System.Text.Json.Serialization;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Server;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(Agent))]
[JsonSerializable(typeof(Weapon))]
[JsonSerializable(typeof(DriveDiscSet))]
[JsonSerializable(typeof(CalcRequest))]
internal partial class SerializerContext : JsonSerializerContext { }