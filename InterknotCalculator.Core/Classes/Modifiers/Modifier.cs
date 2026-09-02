using System.Collections;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Modifiers;

public enum ModifierType {
    /// <summary>
    /// Flat value added to the base value <i>before</i> ratio modifiers scale it.
    /// Weapon main stats and drive disc flat rolls are <see cref="Base"/>.
    /// </summary>
    Base,
    /// <summary>
    /// Ratio scaling the base value and every <see cref="Base"/> modifier.
    /// The <c>*Ratio</c> affixes (ATK%, HP%, DEF%, ...) are <see cref="Multiplicative"/>.
    /// </summary>
    Multiplicative,
    /// <summary>
    /// Flat value added <i>after</i> ratio modifiers have been applied.
    /// Passives that grant a raw amount ("+600 ATK") are <see cref="Additive"/>.
    /// </summary>
    Additive
}

/// <summary>
/// A single contribution to a <see cref="MutableStat"/>.
/// </summary>
public readonly struct Modifier : IEquatable<Modifier> {
    public ModifierKey Key { get; }
    public double Value { get; }
    public ModifierType Type { get; }
    public SkillTag Tags { get; }
    
    /// <summary>
    /// A single contribution to a <see cref="MutableStat"/>.
    /// </summary>
    /// <param name="key">Uniquely identifies the contribution. A stat rejects two modifiers with the same key.</param>
    /// <param name="value">Flat amount or ratio, depending on <paramref name="type"/>.</param>
    /// <param name="type">How the value folds into the stat.</param>
    /// <param name="tags">
    /// Abilities this modifier applies to. <see cref="SkillTag.None"/> means it always applies and folds
    /// into <see cref="MutableStat.Value"/>; anything else makes it conditional and moves it to
    /// <see cref="MutableStat.Tagged"/>.
    /// </param>
    public Modifier(ModifierKey key,
        double value,
        ModifierType type = ModifierType.Additive,
        SkillTag tags = SkillTag.None
    ) {
        Key = key;
        Value = value;
        Type = type;
        Tags = tags;
    }

    public Modifier(ModifierKey key, Stat stat) {
        Key = key;
        Value = stat.Value;
        Type = stat.Affix.IsMultiplicative() ? ModifierType.Multiplicative : ModifierType.Additive;
        Tags = stat.Tags;
    }

    public override string ToString() =>
        $"Mod({Key}, {Value}, {Type} {(Tags is SkillTag.None ? "" : $", {Tags}")})";
    
    public bool Equals(Modifier other) => Key == other.Key;
    public override bool Equals(object? obj) => obj is Modifier other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Key, Value, (int)Type, (int)Tags);
    
    public static bool operator ==(Modifier left, Modifier right) => left.Key == right.Key;
    public static bool operator !=(Modifier left, Modifier right) => !(left == right);
}
