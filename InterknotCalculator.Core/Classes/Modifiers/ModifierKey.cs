using System.Text;

namespace InterknotCalculator.Core.Classes.Modifiers;

/// <summary>
/// A unique identifier for a modifier. Should be enough to
/// understand where a stat mod came from.
/// </summary>
/// <param name="SourceId">Modifier source (agent id, enemy id, etc...)</param>
/// <param name="Name">Modifier name (ability name, passive, etc...)</param>
public readonly record struct ModifierKey(string Name, uint? SourceId = null) {
    public override string ToString() {
        var sb = new StringBuilder(Name);
        if (SourceId.HasValue) {
            sb.Append(':').Append(SourceId.Value);
        }
        return sb.ToString();
    }
    public ModifierKey CombineWith(ModifierKey other) => 
        new($"{Name}:{other.Name}", SourceId ?? other.SourceId);
}
