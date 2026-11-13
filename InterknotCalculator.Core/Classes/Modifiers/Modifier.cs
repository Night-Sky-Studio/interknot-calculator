using System.Diagnostics;
using System.Text.Json.Serialization;

namespace InterknotCalculator.Core.Classes.Modifiers;

/// <summary>
/// A single contribution to a <see cref="MutableStat"/>, created and owned by that stat
/// </summary>
/// <remarks>
/// Everything is immutable except <see cref="Active"/>. Identity is reference identity: keep the
/// instance returned by <see cref="MutableStat.Add(ModifierKey, double, ModifierType, AbilityFilter, Func{bool}?, bool)"/>
/// and toggle <see cref="Active"/> instead of removing and re-adding by key.
/// Never store a modifier in a static field or share it between calculations.
/// </remarks>
public sealed class Modifier {
    /// <summary>
    /// Construction is attachment: the constructor adds the modifier to its owner, so an
    /// unattached or doubly attached modifier can't exist.
    /// Use <see cref="MutableStat.Add(ModifierKey, double, ModifierType, AbilityFilter, Func{bool}?, bool)"/>
    /// </summary>
    internal Modifier(MutableStat owner, ModifierKey key, double value, ModifierType type,
                      AbilityFilter filter, Func<bool>? predicate, bool active) {
        // Conditional modifiers are summed after the fold, whatever their type; see MutableStat.For
        if (!filter.IsAny && type is not (ModifierType.Flat or ModifierType.CombatFlat))
           throw new ArgumentException($"Modifier {key} has a filter, so it must be Flat or CombatFlat, not {type}");

        Owner = owner;
        Key = key;
        Value = value;
        Type = type;
        Filter = filter;
        Predicate = predicate;
        _active = active;
        owner.Attach(this);
    }

    /// <summary>The stat this modifier belongs to</summary>
    [JsonIgnore]
    public MutableStat Owner { get; }

    /// <summary>Where the modifier came from. Not an identity: two modifiers may share a key</summary>
    public ModifierKey Key { get; }

    /// <summary>Flat amount or ratio, depending on <see cref="Type"/></summary>
    public double Value { get; }

    /// <summary>How the value folds into the stat</summary>
    public ModifierType Type { get; }

    /// <summary>
    /// Abilities this modifier applies to. <see cref="AbilityFilter.Any"/> folds it into
    /// <see cref="MutableStat.Value"/>; anything else makes it count only in <see cref="MutableStat.For(Ability)"/>
    /// </summary>
    public AbilityFilter Filter { get; }

    /// <summary>Checked on every read of the owning stat. <c>null</c> means always</summary>
    [JsonIgnore]
    public Func<bool>? Predicate { get; }

    private bool _active;

    /// <summary>Push-style switch. The only mutable member. Changing it refolds the owning stat</summary>
    public bool Active {
        get => _active;
        set {
            if (_active == value) return;
            _active = value;
            Owner.Invalidate();
        }
    }

    /// <summary>Active and the predicate holds. The filter is checked separately, per ability, by the stat</summary>
    [JsonIgnore]
    public bool IsApplied => Active && (Predicate?.Invoke() ?? true);

    internal bool IsConditional => !Filter.IsAny;
    internal bool IsDynamic => Filter.IsAny && Predicate is not null;

    public override string ToString() =>
        $"Mod({Key}, {Value}, {Type}" +
        (Filter.IsAny ? "" : $", {Filter}") +
        (Predicate is null ? "" : ", when") +
        (Active ? "" : ", inactive") + ")";
}
