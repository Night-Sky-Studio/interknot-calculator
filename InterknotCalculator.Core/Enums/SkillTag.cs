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
    CounterAssist = 1 << 10,
    
    Special = 1 << 11, 
    ExSpecial = 1 << 12, 
    Chain = 1 << 13, 
    Ultimate = 1 << 14,
  
    AttributeAnomaly = 1 << 15,
    
    Aftershock = 1 << 16
}