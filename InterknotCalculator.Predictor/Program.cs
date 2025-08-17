using InterknotCalculator.Classes;

namespace InterknotCalculator.Predictor;

public static class Program {
    // private static ImmutableList<Affix> Possible4DiscStats { get; } = [
    //     Affix.AtkRatio,
    //     Affix.CritRate,
    //     Affix.CritDamage,
    //     Affix.AnomalyProficiency,
    // ];
    // private static ImmutableList<Affix> Possible5DiscStats { get; } = [
    //     Affix.AtkRatio,
    //     Affix.PenRatio,
    //     Affix.IceDmgBonus,
    // ];
    // private static ImmutableList<Affix> Possible6DiscStats { get; } = [
    //     Affix.AtkRatio
    // ];

    public static async Task Main() {
        // Initialize shared resources once, prior to optimizer run (thread-safe thereafter)
        await Resources.Current.Init();

        DriveDiscOptimizer.Run();
    }
}