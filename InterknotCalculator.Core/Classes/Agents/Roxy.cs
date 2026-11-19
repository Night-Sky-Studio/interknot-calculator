using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Roxy : SupportAgent, IAgentReference<Roxy> {
    public static Roxy Reference(uint weaponId, uint setId) {
        var roxy = new Roxy();
        
        roxy.InitializeStats(new () {
            [Affix.Atk] = 2500,
            [Affix.CritRate] = 0.76,
            [Affix.CritDamage] = 1,
            [Affix.EnergyRegen] = 3.12
        });

        roxy.SetWeaponPassive(weaponId);
        roxy.SetDriveDiscsPassive(setId);
        
        return roxy;
    }

    public Roxy() : base(AgentId.Roxy) {
        Speciality = Speciality.Stun;
        Element = Element.Wind;
        Rarity = Rarity.S;
        Faction = Faction.Roscaelifer;
        
        InitializeStats(new() {
            [Affix.Hp] = 8418,
            [Affix.Def] = 641,
            [Affix.Atk] = 666 + 75,
            [Affix.CritRate] = 0.05 + 0.144,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 100,
            [Affix.AnomalyMastery] = 110,
            [Affix.AnomalyProficiency] = 63,
            [Affix.EnergyRegen] = 1.2
        });
    }

    public override void RegisterHooks(Context ctx) {
        base.RegisterHooks(ctx);

        ctx.Events.OnCalculationStarted += c => {
            // When Roxy's initial Energy Regen exceeds 1.2, for every 0.01 above this value,
            // ATK increases by 5, up to a maximum increase of 960, and Impact increases by 0.4,
            // up to a maximum increase of 76.8.
            var erOverflow = Math.Max(0, EnergyRegen.InitialValue - 1.2);
            Atk.Add(ModifierKey.Agent(Id) + ModifierKey.CorePassive(),
                Math.Min(erOverflow * 500, 960), ModifierType.CombatFlat);
            Impact.Add(ModifierKey.Agent(Id) + ModifierKey.CorePassive(),
                Math.Min(erOverflow * 40, 76.8), ModifierType.CombatFlat);
            
            // When any squad member triggers Contamination or Cleanse, Roxy provides
            // all squad members with a buff based on the attribute of the Contamination
            // or Cleanse effect:
            foreach (var a in c.Team.Values) {
                var critRate = CritRate.Value;
                
                if (a is ArmorerAgent armorer) {
                    // When Armorer characters deal DMG of the corresponding attribute or Wind DMG,
                    // their Laceration DMG increases by 0.2%, up to 20%.
                    armorer.LacerationDmg.Add(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 
                        Math.Min(critRate * 0.2, 0.2), ModifierType.CombatFlat);
                } else {
                    // ...for every 1% CRIT Rate Roxy has, when non-Armorer characters deal DMG of
                    // the corresponding attribute or Wind DMG, their CRIT DMG increases by 0.4%, up to 40%. 
                    //
                    // TODO(IKC-81): Ability Filter has no Element-based gating
                    //               Applies to all hits, including Physical hits of non-Physical agents
                    a.CritDamage.Add(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 
                        Math.Min(critRate * 0.4, 0.4), ModifierType.CombatFlat);
                }
            }
            
            // TODO(IKC-80): Wind anomaly and Contamination
            var wind = c.Enemy.GetDirectDmgTaken(Element.Wind);
            var contaminated = !c.MainAgent.Element.Matches(Element.Wind)
                ? c.Enemy.GetDirectDmgTaken(c.MainAgent.Element)
                : null;

            if (contaminated is not null) {
                wind.Add(ModifierKey.Contamination(c.MainAgent.Element),
                    0.1, ModifierType.CombatFlat);
                contaminated.Add(ModifierKey.Contamination(c.MainAgent.Element),
                    0.1, ModifierType.CombatFlat);
            }

            // When another character in your squad is an Attack, Rupture, or Armorer character
            if (c.HasTeammates(a => a.Speciality is Speciality.Attack or Speciality.Rupture or Speciality.Armorer, Id)) {
                // Roxy's DMG increases by 8%. This effect increases with character level,
                // increasing by 1.2% per level, up to 80%.
                //
                // 8% + 1.2% * 60 = 8% + 72% = 80%
                DmgBonus.Add(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 0.8, ModifierType.CombatFlat);
                
                // When any squad member's attack hits an enemy,
                // it increases their Stun DMG Multiplier by 30%.
                c.Enemy.StunMultiplier.Add(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 
                    0.30, ModifierType.CombatFlat);

                // While an enemy is in the Windswept state applied by a squad member,
                // the direct Wind DMG they take and the direct DMG they take of the attribute
                // that triggered Contamination are increased by 8%.
                wind.Add(ModifierKey.Contamination(c.MainAgent.Element),
                    0.08, ModifierType.CombatFlat);
                contaminated?.Add(ModifierKey.Contamination(c.MainAgent.Element),
                    0.08, ModifierType.CombatFlat);
            }
        };
    }
}