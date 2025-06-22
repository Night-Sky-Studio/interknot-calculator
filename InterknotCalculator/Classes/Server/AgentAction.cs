using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Server;

public class AgentAction {
    public string Name { get; set; } = "";
    public SkillTag Tag { get; set; }
    public double Damage { get; set; }
    public double Daze { get; set; }
    public ActionStatus Status { get; set; } = ActionStatus.Pass;
}