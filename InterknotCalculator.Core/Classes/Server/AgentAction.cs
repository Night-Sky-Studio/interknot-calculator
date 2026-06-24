using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Server;

public record AgentAction(
    uint AgentId,
    string Name,
    SkillTag Tag,
    double Damage,
    double Daze
) {
    public string Name { get; set; } = Name;
    public double Damage { get; set; } = Damage;
    public double Daze { get; set; } = Daze;
}