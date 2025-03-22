using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Server;

public class AgentAction {
    public string Name { get; set; } = "";
    public SkillTag Tag { get; set; }
    public double Damage { get; set; }
}