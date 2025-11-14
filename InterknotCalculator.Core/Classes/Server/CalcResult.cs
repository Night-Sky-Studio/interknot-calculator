using System.Text;
using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.Enemies;

namespace InterknotCalculator.Core.Classes.Server;

public record CalcResult {
    public FinalStats FinalStats { get; set; } = new();
    public IEnumerable<AgentAction> PerAction { get; set; } = [];
    [JsonIgnore]
    public Enemy? Enemy { get; set; }
    public double Total { get; set; }

    public byte[] Encode() {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding.ASCII, leaveOpen: true);
        
        writer.Write((byte)FinalStats.BaseStats.Count);
        foreach (var (key, value) in FinalStats.BaseStats) {
            writer.Write((byte)key);
            writer.Write((float)value);
        }
        
        writer.Write((byte)FinalStats.CalculatedStats.Count);
        foreach (var (key, value) in FinalStats.CalculatedStats) {
            writer.Write((byte)key);
            writer.Write((float)value);
        }

        var actions = PerAction.ToArray();
        writer.Write((byte)actions.Length);
        foreach (var a in actions) {
            writer.Write((ushort)a.AgentId);
            writer.Write((byte)a.Tag);
            writer.Write(a.Damage);
            var asciiName = Encoding.ASCII.GetBytes(a.Name);
            writer.Write((byte)asciiName.Length);
            writer.Write(asciiName);
        }
        
        writer.Write(Total);
        
        return stream.ToArray();
    }
}