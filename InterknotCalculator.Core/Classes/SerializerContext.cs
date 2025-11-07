using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Core.Classes;

[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(Agent))]
[JsonSerializable(typeof(Weapon))]
[JsonSerializable(typeof(DriveDiscSet))]
[JsonSerializable(typeof(Stat))]
[JsonSerializable(typeof(CalcRequest))]
[JsonSerializable(typeof(CalcResult))]
[JsonSerializable(typeof(AgentAction))]
public partial class SerializerContext : JsonSerializerContext { }