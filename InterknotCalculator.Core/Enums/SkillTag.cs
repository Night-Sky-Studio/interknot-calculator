namespace InterknotCalculator.Core.Enums;

[Flags]
public enum SkillTag {
    None = 0,
    
    DirectHit = 1,
    
    BasicAtk = 2, 
    Dash = 1 << 3, 
    Counter = 1 << 4, 
    
    Entry = 1 << 5, 
    QuickAssist = 1 << 6, 
    DefensiveAssist = 1 << 7, 
    EvasiveAssist = 1 << 8, 
    FollowUpAssist = 1 << 9,
    
    Special = 1 << 10, 
    ExSpecial = 1 << 11, 
    Chain = 1 << 12, 
    Ultimate = 1 << 13,
  
    AttributeAnomaly = 1 << 14,
    
    Aftershock = 1 << 15
}