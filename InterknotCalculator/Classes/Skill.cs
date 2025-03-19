using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes;

public record Scale(double Damage, double Daze, Element? Element = null);

public enum SkillTag {
    BasicAtk, Dash, Counter, 
    QuickAssist, DefensiveAssist, EvasiveAssist, FollowUpAssist,
    Special, ExSpecial, Chain, Ultimate
}

public record Skill {
    public SkillTag Tag { get; set; }
    public List<Scale> Scales { get; set; } = [];
    public SafeDictionary<Affix, double> Affixes { get; set; } = new ();
}