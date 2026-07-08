using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class FusionCompiler : Weapon {
    public FusionCompiler() : base(WeaponId.FusionCompiler) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.S;
        MainStat = new(Affix.Atk, 684);
        SecondaryStat = new(Affix.PenRatio, 0.24);
        Passive = [
            new(Affix.CombatAtkRatio, 0.12),
            new(Affix.AnomalyProficiency, 75)
        ];
    }
}
