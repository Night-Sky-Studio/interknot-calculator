using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class PhaethonsMelody : DriveDiscSet {
    public PhaethonsMelody() : base(DriveDiscSetId.PhaethonsMelody) {
        PartialBonus = [new(Affix.AnomalyMasteryRatio, 0.08)];
        FullBonus = [
            new(Affix.AnomalyProficiency, 45),
            new(Affix.EtherDmgBonus, 0.25)
        ];
    }
}
