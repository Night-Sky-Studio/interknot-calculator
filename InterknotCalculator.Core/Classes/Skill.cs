using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes;

public readonly record struct Scale(
    double Damage,
    double Daze,
    double AnomalyBuildup = 0.0,
    double Energy = 0.0,
    Element? Element = null,
    double GashBuildup = 0.0
);

public readonly record struct Skill(SkillTag Tag, List<Scale> Scales);