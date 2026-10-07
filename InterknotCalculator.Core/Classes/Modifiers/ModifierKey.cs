using System.Text.Json.Serialization;
using InterknotCalculator.Core.Classes.EtherVeils;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Modifiers;

/// <summary>
/// A unique identifier for a modifier. Should be enough to
/// understand where a stat mod came from.
/// </summary>
[JsonConverter(typeof(ModifierKeyJsonConverter))]
public readonly struct ModifierKey : IEquatable<ModifierKey> {
    private string Value { get; }

    public ModifierKey(params string[] components) : this(string.Join(';', components)) {
        foreach (var c in components) {
            if (c.Contains(';'))
                throw new ArgumentException($"ModifierKey component must not contain ';': '{c}'", nameof(components));
        }
    }

    private ModifierKey(string value) => Value = value;

    public override string ToString() =>
        Value ?? throw new ArgumentNullException(nameof(Value), "ModifierKey must be initialized using a constructor.");

    public ModifierKey CombineWith(ModifierKey other) =>
        new(string.Concat(Value, ";", other.Value));

    public bool ComponentStartsWith(string prefix) {
        if (Value is null) return false;

        ReadOnlySpan<char> span = Value;
        while (!span.IsEmpty) {
            var sep = span.IndexOf(';');
            var component = sep < 0 ? span : span[..sep];
            if (component.StartsWith(prefix, StringComparison.Ordinal))
                return true;
            if (sep < 0) break;
            span = span[(sep + 1)..];
        }

        return false;
    }
    public bool Equals(ModifierKey other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
    public override bool Equals(object? obj) => obj is ModifierKey other && Equals(other);
    public override int GetHashCode() => Value?.GetHashCode(StringComparison.Ordinal) ?? 0;
    
    public static bool operator ==(ModifierKey left, ModifierKey right) => left.Equals(right);
    public static bool operator !=(ModifierKey left, ModifierKey right) => !left.Equals(right);
    public static ModifierKey operator +(ModifierKey left, ModifierKey right) => left.CombineWith(right);
    
    public static ModifierKey Agent(uint id) => new($"Agent:{id}");
    public static ModifierKey Weapon(uint id) => new($"Weapon:{id}");
    public static ModifierKey Disc(uint slot) => new($"Disc:{slot}");
    public static ModifierKey Stat(Affix affix, uint level) => new($"Stat:{affix}:{level}");
    public static ModifierKey DiscSet(uint id, bool fullBonus = false) => 
        new($"Disc:Set:{id}:{(fullBonus ? "full" : "partial")}");
    public static ModifierKey EtherVeil() => new("EtherVeil:Any");
    public static ModifierKey EtherVeil<T>() where T : EtherVeil => new($"EtherVeil:{typeof(T).Name}");
    public static ModifierKey Passive() => new("Passive");
    public static ModifierKey CorePassive() => new("CorePassive");
    public static ModifierKey TeamPassive() => new("TeamPassive");
    public static ModifierKey Mindscape(uint level) => new($"Mindscape:{level}");
    public static ModifierKey MainStat() => new("Main");
    public static ModifierKey SecondaryStat() => new("Secondary");
}
