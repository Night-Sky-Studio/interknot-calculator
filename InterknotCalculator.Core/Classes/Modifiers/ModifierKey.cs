using System.Text;

namespace InterknotCalculator.Core.Classes.Modifiers;

/// <summary>
/// A unique identifier for a modifier. Should be enough to
/// understand where a stat mod came from.
/// </summary>
public readonly struct ModifierKey(params string[] components) : IEquatable<ModifierKey> {
    public override string ToString() => string.Join(';', Components);
    public ModifierKey CombineWith(ModifierKey other) => 
        new(ToString(), other.ToString());
    
    private string[] Components { get; init; } = components;
    
    public bool Equals(ModifierKey other) => ToString() == other.ToString();
    public override bool Equals(object? obj) => obj is ModifierKey other && Equals(other);
    public override int GetHashCode() => ToString().GetHashCode();
    
    public static bool operator==(ModifierKey left, ModifierKey right) => left.Equals(right);
    public static bool operator!=(ModifierKey left, ModifierKey right) => !left.Equals(right);
    
    public static ModifierKey operator+(ModifierKey left, ModifierKey right) => left.CombineWith(right);
    
    
    public static ModifierKey Agent(uint id) => new($"Agent:{id}");
    public static ModifierKey Weapon(uint id) => new($"Weapon:{id}");
    public static ModifierKey Disc(uint slot, uint subStat = 0) => new($"Disc:{slot}:{subStat}");
    public static ModifierKey DiscSet(uint id, bool fullBonus = false) => 
        new($"DiscSet:{id}:{(fullBonus ? "full" : "partial")}");
    public static ModifierKey EtherVeil(string name) => new($"EtherVeil:{name}");
}
