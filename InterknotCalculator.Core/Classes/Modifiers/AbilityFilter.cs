using System.Text.Json.Serialization;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Modifiers;

/// <summary>
/// Restricts a <see cref="Modifier"/> to some abilities. Every part that is set must match.
/// </summary>
/// <param name="Tags">Abilities with any of these tags. <see cref="SkillTag.None"/> means any tag.</param>
/// <param name="Names">Abilities with one of these names. <c>null</c> means any name.</param>
/// <param name="Scale">Abilities at this scale. <c>null</c> means any scale.</param>
public readonly record struct AbilityFilter(SkillTag Tags = SkillTag.None, string[]? Names = null, int? Scale = null) {
    public static AbilityFilter Any => default;

    /// <summary>True if the filter matches everything, which makes the modifier unconditional.</summary>
    [JsonIgnore]
    public bool IsAny => Tags is SkillTag.None && Names is null && Scale is null;

    public static AbilityFilter Of(SkillTag tags) => new(tags);
    public static AbilityFilter Of(params string[] names) => new(Names: names);
    public static AbilityFilter Of(string name, int scale) => new(Names: [name], Scale: scale);

    public static implicit operator AbilityFilter(SkillTag tags) => Of(tags);

    /// <summary>Every part that is set must match. Tags match on any overlap.</summary>
    public bool Matches(Ability ability) =>
        (Tags is SkillTag.None || (Tags & ability.Tag) != 0) &&
        (Names is null || Array.IndexOf(Names, ability.Name) >= 0) &&
        (Scale is null || Scale == ability.Scale);

    /// <summary>
    /// For tag-only queries (anomalies, Sunna). A filter that names abilities or a scale
    /// never matches a bare tag.
    /// </summary>
    public bool Matches(SkillTag tag) =>
        Names is null && Scale is null && Tags is not SkillTag.None && (Tags & tag) != 0;

    public override string ToString() {
        if (IsAny) return "Any";
        var parts = new List<string>(3);
        if (Tags is not SkillTag.None) parts.Add(Tags.ToString());
        if (Names is not null) parts.Add(string.Join('|', Names));
        if (Scale is not null) parts.Add($"scale {Scale}");
        return string.Join(", ", parts);
    }
}
