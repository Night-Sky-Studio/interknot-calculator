namespace InterknotCalculator.Interfaces;

public interface IStunAgent {
    public double EnemyStunBonusOverride { get; set; }
    public double GetStandardDaze(string action, int scale);
}