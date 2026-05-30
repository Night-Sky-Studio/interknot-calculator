using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;
using InterknotCalculator.Core.Interfaces;

namespace InterknotCalculator.Core.Classes.Agents;

public class Yuzuha : SupportAgent, IAgentReference<Yuzuha> {
    public static Yuzuha Reference() {
        var yuzuha = new Yuzuha {
            Stats = {
                [Affix.Atk] = 3000,
                [Affix.AnomalyMastery] = 200
            }
        };

        yuzuha.SetWeaponPassive(WeaponId.Metanukimorphosis);
        yuzuha.SetDriveDiscsPassive(DriveDiscSetId.MoonlightLullaby);
        
        yuzuha.ApplyPassive();
        
        return yuzuha;
    }

    private bool SweetScareActive { get; set; } = false;
    
    public Yuzuha() : base(AgentId.Yuzuha) {
        Speciality = Speciality.Support;
        Element = Element.Physical;
        Rarity = Rarity.S;
        Faction = Faction.SpookShack;

        Stats[Affix.Hp] = 8829;
        Stats[Affix.Def] = 612;
        Stats[Affix.Atk] = 758;
        Stats[Affix.CritRate] = 0.05;
        Stats[Affix.CritDamage] = 0.5;
        Stats[Affix.Impact] = 86;
        Stats[Affix.AnomalyMastery] = 124;
        Stats[Affix.AnomalyProficiency] = 93;
        Stats[Affix.EnergyRegen] = 1.2;

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
        ]) {
            Affixes = {
                [Affix.AnomalyBuildupBonus] = 0.25
            }
        };
    }

    public override IEnumerable<AgentAction> GetActionDamage(Context ctx, Ability ability) {
        if (ability.Name is "cavity_alert" or "cavity_alert_right_now") {
            SweetScareActive = true;
        }
        return base.GetActionDamage(ctx, ability);
    }

    public override void RegisterHooks(Context ctx) {
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
            
            c.ActionsQueue.Add(new() {
                AgentId = Id,
                Damage = 0,
                Daze = 0,
                Name = "sugarburst_sparkles_x4",
                Tag = SkillTag.BasicAtk,
            });
            
            var buildup = GetAnomalyBuildup(new(SkillTag.BasicAtk, "sugarburst_sparkles"));
            c.Enemy.AddBuildupContribution(c, this, buildup, e.Agent.Element); // Flavor Match
        });
    }

    public override void ApplyPassive() {
        // Tanuki Wish grants an ATK increase equal to 40% of Yuzuha's initial ATK,
        // up to a maximum increase of 1,200, and increases the DMG dealt by those
        // with the effect by 15%, lasting 40s. Repeated triggers reset the duration.
        ExternalBonus[Affix.Atk] += Math.Min(InitialAtk * 0.4, 1200);
        ExternalBonus[Affix.DmgBonus] += 0.15;
    }

    public override IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) {
        if (team.Count < 2) return [];

        if (team.Any(a => a.Speciality is Speciality.Anomaly || a.Faction == Faction)) {
            // If Yuzuha's Anomaly Mastery exceeds 100, every point over increases
            // the Anomaly Buildup Rate of characters with Tanuki Wish by 0.2%, up
            // to a maximum of 20%, and all Attribute Anomaly DMG and Disorder DMG
            // by 0.2%, up to a maximum of 20%.
            return [
                new(Affix.AnomalyBuildupBonus, 
                    Math.Min(Math.Max(AnomalyMastery - 100, 0) * 0.2, 0.2))
            ];
        }

        return [];
    }
}