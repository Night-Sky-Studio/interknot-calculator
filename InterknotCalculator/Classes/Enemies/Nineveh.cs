namespace InterknotCalculator.Classes.Enemies;

public class Nineveh : Enemy {
    public Nineveh(double stunMultiplier = 1.5) : base(953, 794) {
        Daze = new Progress {
            Maximum = 16747
        };
        StunMultiplier = stunMultiplier;
    }
}