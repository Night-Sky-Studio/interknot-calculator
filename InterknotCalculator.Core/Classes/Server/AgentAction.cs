using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Server;

public record AgentAction {
    public uint AgentId { get; set; }   
    public string Name { get; set; } = "";
    public SkillTag Tag { get; set; }
    public double Damage { get; set; }
    public double Daze { get; set; }
    public ActionStatus Status { get; set; } = ActionStatus.Pass;
}