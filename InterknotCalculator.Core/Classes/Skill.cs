using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

public struct Scale(double damage, double daze, double anomalyBuildup = 0.0, double energy = 0.0, Element? element = null) {
    public double Damage { get; set; } = damage;
    public double Daze { get; set; } = daze;
    public double AnomalyBuildup { get; set; } = anomalyBuildup;
    public double Energy { get; set; } = energy;
    public Element? Element { get; set; } = element;
}

public struct Skill() {
    public Skill(SkillTag tag, IEnumerable<Scale> scales) : this() {
        Tag = tag;
        Scales.AddRange(scales);
    }
    
    public SkillTag Tag { get; set; }
    public List<Scale> Scales { get; set; } = [];
}