using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Enemies;

public abstract class Enemy(double defense, double levelFactor) {
    public double Defense { get; } = defense;
    public double LevelFactor { get; } = levelFactor;
    public Progress Daze { get; set; }
    public double StunMultiplier { get; set; } = 1.5;

    public SafeDictionary<Affix, double> Stats { get; set; } = new();
    
    public double GetDefenseMultiplier(Agent agent) => 
        LevelFactor / (Math.Max(Defense * (1 - agent.PenRatio) - agent.Pen, 0) + LevelFactor);
}