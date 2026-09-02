using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

/// <summary>
/// A single stat: a fixed base value plus a keyed set of <see cref="Modifier"/>s,
/// folded lazily into <see cref="Value"/>.
/// </summary>
/// <remarks>
/// A stat is a reference type, so it is mutated in place and every holder of it sees the same
/// modifiers. Stats are declared once on <see cref="Agents.Agent"/>; an agent only ever replaces
/// one to give it a base value, which is why doing so after modifiers are applied would drop them.
/// </remarks>
public class MutableStat {
    public MutableStat() : this(0) { }

    /// <param name="baseValue">Value the stat has before any modifier is applied.</param>
    public MutableStat(double baseValue) {
        BaseValue = baseValue;
    }

    private List<Modifier> Modifiers { get; } = [];
    private bool Dirty { get; set; } = true;

    public double BaseValue { get; }

    /// <summary>
    /// The folded stat value, counting untagged modifiers only. Tagged modifiers are
    /// ability-conditional and belong to <see cref="Tagged"/> instead.
    /// </summary>
    public double Value {
        get {
            if (Dirty) Fold();
            return field;
        }
        private set;
    } = 0;

    public IReadOnlyList<Modifier> AppliedModifiers => Modifiers;

    /// <summary>
    /// Applies a modifier to this stat.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// A modifier with the same <see cref="ModifierKey"/> is already applied. Every source is
    /// expected to apply itself exactly once, so a collision means a bonus is about to be
    /// counted twice.
    /// </exception>
    public void Add(Modifier modifier) {
        if (Modifiers.Exists(m => m == modifier)) {
            throw new ArgumentException(
                $"Modifier {modifier.Key} is already applied to this stat", nameof(modifier));
        }
        Modifiers.Add(modifier);
        Dirty = true;
    }

    public void Remove(Modifier modifier) {
        if (Modifiers.Remove(modifier)) Dirty = true;
    }

    public void RemoveKey(ModifierKey key) {
        if (Modifiers.RemoveAll(m => m.Key == key) > 0) Dirty = true;
    }

    public bool Has(ModifierKey key) => Modifiers.Exists(m => m.Key == key);

    /// <summary>
    /// Drops every modifier, keeping <see cref="BaseValue"/>. Used when equipment is re-applied,
    /// so that sources can add themselves again without colliding.
    /// </summary>
    public void Clear() {
        if (Modifiers.Count == 0) return;
        Modifiers.Clear();
        Dirty = true;
    }

    /// <summary>
    /// Sum of the modifiers that only apply to <paramref name="tag"/>. Tagged modifiers are flat
    /// contributions, so they are summed rather than folded - every conditional bonus in the game
    /// data is a flat one.
    /// </summary>
    public double Tagged(SkillTag tag) {
        if (tag is SkillTag.None) return 0;
        var total = 0.0;
        foreach (var m in Modifiers) {
            if (m.Tags is not SkillTag.None && (m.Tags & tag) != 0) total += m.Value;
        }
        return total;
    }

    /// <summary><see cref="Value"/> plus the modifiers conditional on <paramref name="tag"/>.</summary>
    public double For(SkillTag tag) => Value + Tagged(tag);

    private void Fold() {
        var baseSum = 0.0;
        var multiplicative = 0.0;
        var additive = 0.0;

        foreach (var m in Modifiers) {
            // Tagged modifiers only count for the abilities they name; see Tagged().
            if (m.Tags is not SkillTag.None) continue;
            switch (m.Type) {
                case ModifierType.Base: baseSum += m.Value; break;
                case ModifierType.Multiplicative: multiplicative += m.Value; break;
                case ModifierType.Additive: additive += m.Value; break;
            }
        }

        Value = (BaseValue + baseSum) * (1 + multiplicative) + additive;
        Dirty = false;
    }

    public static MutableStat operator +(MutableStat left, Modifier right) {
        left.Add(right);
        return left;
    }

    public static MutableStat operator -(MutableStat left, Modifier right) {
        left.Remove(right);
        return left;
    }

    public static implicit operator double(MutableStat stat) => stat.Value;

    public override string ToString() => $"MutableStat({BaseValue} -> {Value}, {Modifiers.Count} mods)";
}
