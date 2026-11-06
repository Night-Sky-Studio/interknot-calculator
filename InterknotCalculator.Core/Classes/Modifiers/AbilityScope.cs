namespace InterknotCalculator.Core.Classes.Modifiers;

/// <summary>
/// Restricts a modifier to a specific ability
/// </summary>
public readonly record struct AbilityScope(string[]? Names = null, int? Scale = null) {
    public static AbilityScope Any => default;
    public bool IsAny => Names is null && Scale is null;
    
    public static AbilityScope Of(params string[] names) => new(names);
    public static AbilityScope Of(string name, int? scale = null) => new([name], scale);

    public bool Matches(Ability ability) =>
        (Names is null || Array.IndexOf(Names, ability.Name) >= 0) &&
        (Scale is null || Scale == ability.Scale);
}