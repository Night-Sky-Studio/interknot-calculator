namespace InterknotCalculator.Core.Enums;

[Flags]
public enum Faction {
    None = 0,
    BelobogHeavyIndustries = 1,
    CriminalInvestigationSpecialResponseTeam = 1 << 1,
    CunningHares = 1 << 2,
    SilverSquad = 1 << 3,
    HollowSpecialOperationsSection6 = 1 << 4,
    LyreSquad = 1 << 5,
    Mockingbird = 1 << 6,
    ObolSquad = 1 << 7,
    SonsOfCalydon = 1 << 8,
    StarsOfLyra = 1 << 9,
    VictoriaHousekeeping = 1 << 10,
    YunkuiSummit = 1 << 11,
    SpookShack = 1 << 12,
    KrampusComplianceAuthority = 1 << 13,
    NewEriduDefenseForce = SilverSquad | ObolSquad | LyreSquad
}