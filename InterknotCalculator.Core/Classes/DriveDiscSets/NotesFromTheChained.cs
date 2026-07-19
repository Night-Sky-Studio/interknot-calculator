using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.DriveDiscSets;

public class NotesFromTheChained : DriveDiscSet {
    public NotesFromTheChained() : base(DriveDiscSetId.NotesFromTheChained) {
        PartialBonus = [new(Affix.IceDmgBonus, 0.1)];
        FullBonus = [
            new(Affix.AnomalyProficiency, 48),
            new(Affix.AnomalyDmgBonus, 0.16)
        ];
    }
}
