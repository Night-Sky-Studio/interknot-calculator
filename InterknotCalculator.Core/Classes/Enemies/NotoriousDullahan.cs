namespace InterknotCalculator.Core.Classes.Enemies;

public class NotoriousDullahan : Enemy {
    public NotoriousDullahan(double stunMultiplier = 1.5) : base(953, 794, 2250) {
        Daze = new Progress {
            Maximum = 9257
        };
        StunMultiplier = stunMultiplier;
    }
}