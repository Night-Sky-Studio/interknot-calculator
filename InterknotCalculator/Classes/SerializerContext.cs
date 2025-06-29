using System.Text.Json.Serialization;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Classes.Server;

namespace InterknotCalculator.Classes;

[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(Agent))]
[JsonSerializable(typeof(Weapon))]
[JsonSerializable(typeof(DriveDiscSet))]
[JsonSerializable(typeof(Stat))]
[JsonSerializable(typeof(CalcRequest))]
[JsonSerializable(typeof(CalcResult))]
[JsonSerializable(typeof(AgentAction))]
internal partial class SerializerContext : JsonSerializerContext { }