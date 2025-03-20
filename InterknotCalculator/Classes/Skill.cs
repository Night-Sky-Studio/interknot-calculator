using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public record Scale(double Damage, double Daze, Element? Element = null);

public record Skill {
    public SkillTag Tag { get; set; }
    public List<Scale> Scales { get; set; } = [];
    public SafeDictionary<Affix, double> Affixes { get; set; } = new ();
}