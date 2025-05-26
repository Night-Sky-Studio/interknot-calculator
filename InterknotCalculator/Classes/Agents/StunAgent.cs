using InterknotCalculator.Enums;
using InterknotCalculator.Interfaces;

namespace InterknotCalculator.Classes.Agents;

public abstract class StunAgent : Agent, IStunAgent {
    public abstract double EnemyStunBonusOverride { get; set; }
    public double GetStandardDaze(string action, int scale) {
        var data = Skills[action];

        var tagDazeBonus = 1.0;
        foreach (var stat in TagBonus) {
            if (stat.SkillTags.Contains(data.Tag) && stat.Affix == Affix.DazeBonus) {
                tagDazeBonus += stat.Value;
            }
        }

        var abilityPassive = ApplyAbilityPassive(action);
        if (abilityPassive is { Affix: Affix.DazeBonus } passive) {
            tagDazeBonus += passive.Value;
        }
        
        var dazeDmgMultiplier = data.Scales[scale].Daze;
        var dazeIncrease = 0.0 + tagDazeBonus;
        var dazeReduction = 0.0;
        var dazeRes = 0.0;
        var dazeTakenIncrease = 0.0;
        var dazeTakenReduction = 0.0;
        return Impact * dazeDmgMultiplier * (1 + dazeIncrease - dazeReduction)
            * (1 - dazeRes) * (1 + dazeTakenIncrease - dazeTakenReduction);
    }
}