using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Weapons;

public class WeepingGemini : Weapon {
    public WeepingGemini() : base(WeaponId.WeepingGemini) {
        Speciality = Speciality.Anomaly;
        Rarity = Rarity.A;
        MainStat = new(Affix.Atk, 594);
        SecondaryStat = new(Affix.AtkRatio, 0.25);
        Passive = [new(Affix.AnomalyProficiency, 184)];
    }
}
