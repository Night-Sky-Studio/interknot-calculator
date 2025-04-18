namespace InterknotCalculator.Enums;

[Flags]
public enum Faction {
    BelobogHeavyIndustries = 0,
    CriminalInvestigationResponseTeam = 1,
    CunningHares = 2,
    SilverSquad = 4,
    HollowSpecialOperationsSection6 = 8,
    LyreSquad = 16,
    Mockingbird = 32,
    ObolSquad = 64,
    SonsOfCalydon = 128,
    StarsOfLyra = 256,
    VictoriaHousekeeping = 512,
    NewEriduDefenseForce = SilverSquad | ObolSquad | LyreSquad
}