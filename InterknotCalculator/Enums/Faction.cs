namespace InterknotCalculator.Enums;

[Flags]
public enum Faction {
    BelobogHeavyIndustries,
    CriminalInvestigationResponseTeam,
    CunningHares,
    SilverSquad,
    HollowSpecialOperationsSection6,
    LyreSquad,
    Mockingbird,
    ObolSquad,
    SonsOfCalydon,
    StarsOfLyra,
    VictoriaHousekeeping,
    NewEriduDefenseForce = SilverSquad | ObolSquad | LyreSquad
}