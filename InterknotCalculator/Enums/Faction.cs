namespace InterknotCalculator.Enums;

[Flags]
public enum Faction {
    None = 0,
    BelobogHeavyIndustries = 1,
    CriminalInvestigationSpecialResponseTeam = 2,
    CunningHares = 4,
    SilverSquad = 8,
    HollowSpecialOperationsSection6 = 16,
    LyreSquad = 32,
    Mockingbird = 64,
    ObolSquad = 128,
    SonsOfCalydon = 256,
    StarsOfLyra = 512,
    VictoriaHousekeeping = 1024,
    NewEriduDefenseForce = SilverSquad | ObolSquad | LyreSquad
}