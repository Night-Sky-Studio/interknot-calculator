using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

/// <summary>
/// A single stat: a fixed base value plus the <see cref="Modifier"/>s it owns,
/// folded lazily into <see cref="Value"/>.
/// </summary>
/// <remarks>
/// <para>
/// A stat is a reference type, so it is mutated in place and every holder of it sees the same
/// modifiers. Stats are declared once on <see cref="Agents.Agent"/>; an agent only ever replaces
/// one to give it a base value, which is why doing so after modifiers are applied would drop them.
/// </para>
/// <para>
/// Modifiers fall into three groups:
/// <list type="bullet">
/// <item><b>Static</b> (no filter, no predicate): summed once and cached until a modifier is
/// attached, removed or toggled.</item>
/// <item><b>Dynamic</b> (no filter, a predicate): added on top of the cache on every read of
/// <see cref="Value"/>/<see cref="InitialValue"/>.</item>
/// <item><b>Conditional</b> (a filter): only counted by <see cref="For(Ability)"/> and
/// <see cref="For(SkillTag)"/>, as flat values after the fold.</item>
/// </list>
/// Inactive modifiers are excluded from every group.
/// </para>
/// </remarks>
public class MutableStat {
    public MutableStat() : this(0) { }

    /// <param name="baseValue">Value the stat has before any modifier is applied.</param>
    public MutableStat(double baseValue) {
        BaseValue = baseValue;
    }

    private readonly List<Modifier> _modifiers = [];
    private bool _dirty = true;
    private int _dynamicCount;

    // Static sums, cached until Invalidate().
    private double _base, _ratio, _flat, _combatRatio, _combatFlat;
    private double _cachedInitialValue, _cachedValue;

    public double BaseValue { get; }

    /// <summary>Number of times the static sums were recomputed. For tests.</summary>
    internal int FoldCount { get; private set; }

    /// <summary>
    /// The folded stat value, counting unconditional modifiers only. Modifiers with a filter
    /// belong to <see cref="For(Ability)"/> instead.
    /// </summary>
    public double Value {
        get {
            if (_dirty) Fold();
            if (_dynamicCount == 0) return _cachedValue;
            return FoldDynamic().Value;
        }
    }

    /// <summary><see cref="Value"/> without the combat modifiers.</summary>
    public double InitialValue {
        get {
            if (_dirty) Fold();
            if (_dynamicCount == 0) return _cachedInitialValue;
            return FoldDynamic().InitialValue;
        }
    }

    /// <summary>Every modifier attached to this stat, including inactive ones.</summary>
    public IReadOnlyList<Modifier> Modifiers => _modifiers;

    /// <summary>
    /// Creates a modifier owned by this stat. Keep the returned instance to toggle
    /// <see cref="Modifier.Active"/> later.
    /// </summary>
    /// <param name="key">Where the bonus came from.</param>
    /// <param name="value">Flat amount or ratio, depending on <paramref name="type"/>.</param>
    /// <param name="type">How the value folds into the stat.</param>
    /// <param name="filter">Abilities the modifier applies to. Must be Flat or CombatFlat when set.</param>
    /// <param name="when">Checked on every read. <c>null</c> means always.</param>
    /// <param name="active">Initial state of <see cref="Modifier.Active"/>.</param>
    public Modifier Add(ModifierKey key, double value, ModifierType type,
                        AbilityFilter filter = default, Func<bool>? when = null, bool active = true)
        => new(this, key, value, type, filter, when, active);

    /// <summary>
    /// Creates a modifier from an equipment or disc stat. Ratio affixes become (Combat)Ratio,
    /// everything else (Combat)Flat; the stat's tags become the filter.
    /// </summary>
    public Modifier Add(ModifierKey key, Stat stat, bool combat = false,
                        Func<bool>? when = null, bool active = true) {
        var type = stat.Affix.IsRatio()
            ? combat ? ModifierType.CombatRatio : ModifierType.Ratio
            : combat ? ModifierType.CombatFlat : ModifierType.Flat;
        return Add(key, stat.Value, type, AbilityFilter.Of(stat.Tags), when, active);
    }

    /// <summary>Creates a modifier with the negated <paramref name="value"/>.</summary>
    public Modifier Subtract(ModifierKey key, double value, ModifierType type,
                             AbilityFilter filter = default, Func<bool>? when = null, bool active = true)
        => Add(key, -value, type, filter, when, active);

    /// <summary>
    /// Temporary: adds a modifier unless one with the same key and filter is already attached.
    /// </summary>
    /// <exception cref="ArgumentException">A modifier with the same key and filter exists.</exception>
    public Modifier AddUnique(ModifierKey key, double value, ModifierType type,
                              AbilityFilter filter = default, Func<bool>? when = null, bool active = true) {
        if (_modifiers.Exists(m => m.Key == key && m.Filter == filter)) {
            throw new ArgumentException($"Modifier {key} is already applied to this stat", nameof(key));
        }
        return Add(key, value, type, filter, when, active);
    }

    /// <summary>Permanently detaches <paramref name="modifier"/> from this stat.</summary>
    public void Remove(Modifier modifier) {
        if (!_modifiers.Remove(modifier)) return;
        if (modifier.IsDynamic) _dynamicCount--;
        Invalidate();
    }

    /// <summary>Detaches every modifier with <paramref name="key"/>. A provenance query.</summary>
    public void RemoveKey(ModifierKey key) {
        for (var i = _modifiers.Count - 1; i >= 0; i--) {
            if (_modifiers[i].Key == key) Remove(_modifiers[i]);
        }
    }

    public bool Contains(ModifierKey key) => _modifiers.Exists(m => m.Key == key);

    /// <summary>
    /// Drops every modifier, keeping <see cref="BaseValue"/>.
    /// </summary>
    public void Clear() {
        if (_modifiers.Count == 0) return;
        _modifiers.Clear();
        _dynamicCount = 0;
        Invalidate();
    }

    /// <summary>Called only by the <see cref="Modifier"/> constructor.</summary>
    internal void Attach(Modifier modifier) {
        _modifiers.Add(modifier);
        if (modifier.IsDynamic) _dynamicCount++;
        Invalidate();
    }

    /// <summary>Called when a modifier is attached, removed or toggled.</summary>
    internal void Invalidate() => _dirty = true;

    /// <summary><see cref="Value"/> plus the applied modifiers whose filter matches <paramref name="tag"/>.</summary>
    /// <remarks>Filters that name abilities never match a bare tag.</remarks>
    public double For(SkillTag tag) => Value + Tagged(tag);

    /// <summary><see cref="Value"/> plus the applied modifiers whose filter matches <paramref name="ability"/>.</summary>
    public double For(Ability ability) => Value + Conditional(ability);

    /// <summary>
    /// Sum of the conditional modifiers on <paramref name="tag"/>. Conditional modifiers are flat
    /// contributions, so they are summed rather than folded.
    /// </summary>
    private double Tagged(SkillTag tag) {
        var total = 0.0;
        foreach (var m in _modifiers) {
            if (m.IsConditional && m.Filter.Matches(tag) && m.IsApplied) total += m.Value;
        }
        return total;
    }

    /// <summary>Sum of the conditional modifiers on <paramref name="ability"/>.</summary>
    private double Conditional(Ability ability) {
        var total = 0.0;
        foreach (var m in _modifiers) {
            if (m.IsConditional && m.Filter.Matches(ability) && m.IsApplied) total += m.Value;
        }
        return total;
    }

    private void Fold() {
        double baseSum = 0.0, ratioSum = 0.0, flatSum = 0.0, combatRatioSum = 0.0, combatFlatSum = 0.0;

        foreach (var m in _modifiers) {
            if (!m.Active || m.IsConditional || m.Predicate is not null) continue;
            switch (m.Type) {
                case ModifierType.Base: baseSum += m.Value; break;
                case ModifierType.Ratio: ratioSum += m.Value; break;
                case ModifierType.Flat: flatSum += m.Value; break;
                case ModifierType.CombatRatio: combatRatioSum += m.Value; break;
                case ModifierType.CombatFlat: combatFlatSum += m.Value; break;
            }
        }

        (_base, _ratio, _flat, _combatRatio, _combatFlat) = (baseSum, ratioSum, flatSum, combatRatioSum, combatFlatSum);
        (_cachedInitialValue, _cachedValue) = Compose(baseSum, ratioSum, flatSum, combatRatioSum, combatFlatSum);
        _dirty = false;
        FoldCount++;
    }

    /// <summary>The cached static sums plus the dynamic modifiers whose predicate holds right now.</summary>
    private (double InitialValue, double Value) FoldDynamic() {
        double baseSum = _base, ratioSum = _ratio, flatSum = _flat,
               combatRatioSum = _combatRatio, combatFlatSum = _combatFlat;

        foreach (var m in _modifiers) {
            if (!m.IsDynamic || !m.IsApplied) continue;
            switch (m.Type) {
                case ModifierType.Base: baseSum += m.Value; break;
                case ModifierType.Ratio: ratioSum += m.Value; break;
                case ModifierType.Flat: flatSum += m.Value; break;
                case ModifierType.CombatRatio: combatRatioSum += m.Value; break;
                case ModifierType.CombatFlat: combatFlatSum += m.Value; break;
            }
        }

        return Compose(baseSum, ratioSum, flatSum, combatRatioSum, combatFlatSum);
    }

    private (double InitialValue, double Value) Compose(double baseSum, double ratioSum, double flatSum,
                                                        double combatRatioSum, double combatFlatSum) {
        var initialValue = (BaseValue + baseSum) * (1 + ratioSum) + flatSum;
        return (initialValue, initialValue * (1 + combatRatioSum) + combatFlatSum);
    }

    public static implicit operator double(MutableStat stat) => stat.Value;

    public override string ToString() => $"MutableStat({BaseValue} -> {Value}, {_modifiers.Count} mods)";
}
