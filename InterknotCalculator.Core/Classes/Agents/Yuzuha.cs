using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Yuzuha : SupportAgent, IAgentReference<Yuzuha> {
    public static Yuzuha Reference(uint weaponId, uint setId) {
        var yuzuha = new Yuzuha();
        
        yuzuha.InitializeStats(new () {
            [Affix.Atk] = 3000,
            [Affix.AnomalyMastery] = 200
        });

        yuzuha.SetWeaponPassive(weaponId);
        yuzuha.SetDriveDiscsPassive(setId);
        
        return yuzuha;
    }

    private bool SweetScareActive { get; set; }
    
    public Yuzuha() : base(AgentId.Yuzuha) {
        Speciality = Speciality.Support;
        Element = Element.Physical;
        Rarity = Rarity.S;
        Faction = Faction.SpookShack;

        InitializeStats(new () {
            [Affix.Hp] = 8829,
            [Affix.Def] = 612,
            [Affix.Atk] = 758,
            [Affix.CritRate] = 0.05,
            [Affix.CritDamage] = 0.5,
            [Affix.Impact] = 86,
            [Affix.AnomalyMastery] = 124,
            [Affix.AnomalyProficiency] = 93,
            [Affix.EnergyRegen] = 1.2
        });

        Skills["cavity_alert"] = new(SkillTag.ExSpecial, [
            new(842.3, 632.2, 482.08, -60)
        ]);
        Skills["cavity_alert_right_now"] = new(SkillTag.ExSpecial, [
            new(483.4, 401.2, 324.62, -60)
        ]);
        
        // Enemies under the effect of Sweet Scare will be attacked by a
        // Basic Attack: Sugarburst Sparkles once every 1s.
        // 
        // To get any meaningful Anomaly Buildup buff, we estimate
        // it by making 4 triggers of Sugarburst Sparkles.
        Skills["sugarburst_sparkles"] = new(SkillTag.BasicAtk, [
            new(55, 0, anomalyBuildup: 17.66 * 4)
        ], new () {
            [Affix.AnomalyBuildupBonus] = 0.25
        });
    }

    public override IEnumerable<AgentAction> GetActionDamage(Context ctx, Ability ability) {
        if (ability.Name is "cavity_alert" or "cavity_alert_right_now") {
            SweetScareActive = true;
        }
        return base.GetActionDamage(ctx, ability);
    }

    public override void RegisterHooks(Context ctx) {
        ctx.Events.OnCalculationStarted.Add(c => {
            foreach (var agent in c.Team.Values) {
                // Tanuki Wish grants an ATK increase equal to 40% of Yuzuha's initial ATK,
                // up to a maximum increase of 1,200, and increases the DMG dealt by those
                // with the effect by 15%, lasting 40s. Repeated triggers reset the duration.
                agent.Atk.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 
                    Math.Min(Atk.InitialValue * 0.4, 1200)));
                agent.DmgBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.CorePassive(), 0.15));
            }

            if (c.Team.Values.Any(a => a.Speciality is Speciality.Anomaly || a.Faction == Faction)) {
                // If Yuzuha's Anomaly Mastery exceeds 100, every point over increases
                // the Anomaly Buildup Rate of characters with Tanuki Wish by 0.2%, up
                // to a maximum of 20%, and all Attribute Anomaly DMG and Disorder DMG
                // by 0.2%, up to a maximum of 20%.
                AnomalyBuildupBonus.Add(new(ModifierKey.Agent(Id) + ModifierKey.TeamPassive(), 
                    Math.Min(Math.Max(AnomalyMastery - 100, 0) * 0.002, 0.2)));
            }
        });
        
        ctx.Events.OnActionExecuted.Add((c, e) => {
            // The Sweet Scare state lasts for 40.0s, repeated triggers reset the duration.
            //
            // When an enemy affected by Sweet Scare is first hit by an active character
            // using an attack of their attribute, Flavor Match is triggered against that
            // enemy, changing the attribute of Basic Attack: Sugarburst Sparkles and
            // Basic Attack: Sugarburst Sparkles - Max against that enemy to match that of
            // the character who triggered Flavor Match.
            //
            // When Sweet Scare is triggered again on an enemy, their existing Flavor Match
            // state is removed. The process can be repeated to change the attribute of Basic
            // Attack: Sugarburst Sparkles and Basic Attack: Sugarburst Sparkles - Max again.
            if (e.Agent == this) return;
            if (!SweetScareActive) return;
            if (e.Ability.Tag is SkillTag.AttributeAnomaly
                or SkillTag.DefensiveAssist
                or SkillTag.Aftershock) return;
            
            c.ActionsQueue.Add(new(Id, "sugarburst_sparkles_x4", SkillTag.BasicAtk, 0, 0));
            
            var buildup = GetAnomalyBuildup(new(SkillTag.BasicAtk, "sugarburst_sparkles"));
            c.Enemy.AddBuildupContribution(c, this, buildup, e.Agent.Element); // Flavor Match
        });
    }
}