namespace InterknotCalculator.Core.Classes.Modifiers;

public enum ModifierType {
    /// <summary>
    /// Flat value added to the base value <i>before</i> ratio modifiers scale it.
    /// Weapon main stats and drive disc flat rolls are <see cref="Base"/>.
    /// </summary>
    Base,
    /// <summary>
    /// Ratio scaling the base value and every <see cref="Base"/> modifier.
    /// The <c>*Ratio</c> affixes (ATK%, HP%, DEF%, ...) are <see cref="Ratio"/>.
    /// </summary>
    Ratio,
    /// <summary>
    /// Flat value added <i>after</i> ratio modifiers have been applied.
    /// Passives that grant a raw amount ("+600 ATK") are <see cref="Flat"/>.
    /// </summary>
    Flat,
    CombatRatio,
    CombatFlat
}
