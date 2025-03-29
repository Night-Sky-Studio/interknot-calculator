using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public record Scale(double Damage, double Daze, Element? Element = null);

public record Skill() {
    public Skill(SkillTag tag, IEnumerable<Scale> scales, SafeDictionary<Affix, double>? affixes = null) : this() {
        Tag = tag;
        Scales.AddRange(scales);
        if (affixes != null) {
            Affixes = affixes;
        }
    }
    public SkillTag Tag { get; set; }
    public List<Scale> Scales { get; set; } = [];
    public SafeDictionary<Affix, double> Affixes { get; set; } = new ();
}