using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Classes.Server;

namespace InterknotCalculator.Core.Classes;

[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(Agent))]
[JsonSerializable(typeof(Stat))]
[JsonSerializable(typeof(Enemy))]
[JsonSerializable(typeof(CalcRequest))]
[JsonSerializable(typeof(CalcResult))]
[JsonSerializable(typeof(MutableStat))]
[JsonSerializable(typeof(Modifier))]
[JsonSerializable(typeof(ModifierKey))]
public partial class SerializerContext : JsonSerializerContext;