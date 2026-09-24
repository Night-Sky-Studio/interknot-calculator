using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Modifiers;

/// <summary>
/// A unique identifier for a modifier. Should be enough to
/// understand where a stat mod came from.
/// </summary>
public readonly struct ModifierKey(params string[] components) : IEquatable<ModifierKey> {
    public override string ToString() => string.Join(';', Components);
    public ModifierKey CombineWith(ModifierKey other) => 
        new([..Components, ..other.Components]);
    
    private string[] Components { get; } = components;
    
    public bool StartsWith(string prefix) => Components.FirstOrDefault()?.StartsWith(prefix) ?? false;
    
    public bool Equals(ModifierKey other) => ToString() == other.ToString();
    public override bool Equals(object? obj) => obj is ModifierKey other && Equals(other);
    public override int GetHashCode() => ToString().GetHashCode();
    
    public static bool operator==(ModifierKey left, ModifierKey right) => left.Equals(right);
    public static bool operator!=(ModifierKey left, ModifierKey right) => !left.Equals(right);
    
    public static ModifierKey operator+(ModifierKey left, ModifierKey right) => left.CombineWith(right);
    
    
    public static ModifierKey Agent(uint id) => new($"Agent:{id}");
    public static ModifierKey Weapon(uint id) => new($"Weapon:{id}");
    public static ModifierKey Disc(uint slot) => new($"Disc:{slot}");
    public static ModifierKey Stat(Affix affix, uint level) => new($"Stat:{affix}:{level}");
    public static ModifierKey DiscSet(uint id, bool fullBonus = false) => 
        new($"DiscSet:{id}:{(fullBonus ? "full" : "partial")}");
    public static ModifierKey EtherVeil(string name) => new($"EtherVeil:{name}");
    public static ModifierKey Passive() => new("Passive");
    public static ModifierKey CorePassive() => new("CorePassive");
    public static ModifierKey TeamPassive() => new("TeamPassive");
    public static ModifierKey Mindscape(uint level) => new($"Mindscape:{level}");
    public static ModifierKey MainStat() => new("Main");
    public static ModifierKey SecondaryStat() => new("Secondary");
}
