using System.Collections.Immutable;
using InterknotCalculator.Core.Classes.DriveDiscSets;
using InterknotCalculator.Core.Classes.Enemies;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Classes.Weapons;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Core.Classes.Agents;

/// <summary>
/// Base Agent class
/// </summary>
public abstract class Agent(uint id) {
    private const double DamageTakenMultiplier = 1;

    #region Information
    public uint Id { get; } = id;
    public Speciality Speciality { get; set; }
    public Element Element { get; set; }
    public Rarity Rarity { get; set; }
    public Faction Faction { get; set; }
    #endregion
    
    #region Collections
    public StatsDictionary Stats { get; set; } = new();
    public Dictionary<Element, Anomaly> Anomalies { get; set; } = new();
    public Dictionary<string, Skill> Skills { get; set; } = new();
    public Dictionary<string, IEnumerable<string>> Macros { get; set; } = new();
    #endregion

    #region Equipment

    public Weapon? Weapon { get; private set; }
    public DriveDisc[] DriveDiscs { get; private set; } = [];

    public void SetWeapon(uint weaponId) {
        RemoveWeaponStats();
        Weapon = WeaponRegistry.CreateInstance(weaponId);
        AddWeaponStats();
    }
    private void RemoveWeaponStats() {
        if (Weapon is not { } w) 
            return;
        Stats[w.MainStat.Affix].RemoveKey(w.MainStatKey);
        Stats[w.SecondaryStat.Affix].RemoveKey(w.SecondaryStatKey);
        if (w.Speciality != Speciality) 
            return;
        foreach (var passive in w.Passive) {
            Stats[passive.Key].Remove(passive.Value);
        }
        foreach (var external in w.ExternalBonus) {
            Stats[external.Key].Remove(external.Value);
        }
    }
    private void AddWeaponStats() {
        if (Weapon is not { } w)
            return;
        Stats[w.MainStat.Affix].Add(new(w.MainStatKey, w.MainStat.Value, ModifierType.Base));
        Stats[w.SecondaryStat.Affix].Add(new(w.SecondaryStatKey, w.SecondaryStat));
        if (w.Speciality != Speciality) 
            return;
        foreach (var passive in w.Passive) {
            Stats[passive.Key].Add(passive.Value);
        }
        foreach (var external in w.ExternalBonus) {
            Stats[external.Key].Add(external.Value);
        }
    }

    public void SetDriveDiscs(DriveDisc[] discs) {
        RemoveDiscsStats();
        DriveDiscs = discs;
        AddDiscsStats();
    }
    private void RemoveDiscsStats() {
        foreach (var value in Stats.Values) {
            var toRemove = value.AppliedModifiers
                .Where(m => m.Key.ToString().StartsWith("disc"))
                .ToImmutableList(); // freeze mods
            foreach (var mod in toRemove) {
                value.Remove(mod);
            }
        }
    }
    private void AddDiscsStats() {
        var setCounts = new SafeDictionary<uint, int>();

        foreach (var disc in DriveDiscs) {
            setCounts[disc.SetId] += 1;
            Stats[disc.MainStat.Affix] += new Modifier(disc.Key, disc.MainStat);
            foreach (var subStat in disc.SubStats) {
                Stats[subStat.Affix] += new Modifier(disc.Key.CombineWith(subStat.Key), subStat.Value);
            }
        }
        
        var partialSets = setCounts
            .Where(kvp => kvp.Value >= 2)
            .Select(kvp => kvp.Key);
        
        foreach (var setId in partialSets) {
            var set = DriveDiscSetRegistry.CreateInstance(setId);
            foreach (var bonus in set.PartialBonus) {
                Stats[bonus.Affix] += new Modifier(set.Key.CombineWith(bonus.Key), bonus);
            }
        }
        
        var fullSets = setCounts
            .Where(kvp => kvp.Value >= 4)
            .Select(kvp => kvp.Key);
        
        foreach (var setId in fullSets) {
            var set = DriveDiscSetRegistry.CreateInstance(setId);
            foreach (var bonus in set.FullBonus) {
                Stats[bonus.Affix] += new Modifier(set.Key.CombineWith(bonus.Key), bonus);
            }
        }
    }
    #endregion
    
    #region Stats
    public SafeDictionary<Affix, double> BaseStats { get; private set; } = new();
    
    private SafeDictionary<Affix, double>? _finalStats;
    public SafeDictionary<Affix, double> FinalStats {
        // Lazily snapshot stats when first accessed.
        // This way Reference agents that don't go through SetWeapon/SetDriveDiscs
        // (and therefore never trigger ProcessStats) still get a valid snapshot
        // without forcing every implementation to remember to snapshot manually.
        get => _finalStats ??= CollectStats();
        private set => _finalStats = value;
    }
    
    public Affix RelatedElementDmg => Helpers.GetRelatedAffixDmg(Element);
    public Affix RelatedElementRes => Helpers.GetRelatedAffixRes(Element);

    public double MaxHp => Stats[Affix.Hp];
    private double _hp;
    public double Hp {
        get => Math.Clamp(_hp, 0, MaxHp); 
        set => _hp = Math.Clamp(value, 0, MaxHp);
    }
    public double InitialAtk => Stats[Affix.Atk];
    public double Atk => InitialAtk * (1 + Stats[Affix.CombatAtkRatio]);
    public double Def => Stats[Affix.Def];
    public double Pen => Stats[Affix.Pen];
    public double PenRatio => Stats[Affix.PenRatio];
    public double CritRate => Stats[Affix.CritRate];
    public double CritDamage => Stats[Affix.CritDamage];
    public double Impact => Stats[Affix.Impact];
    public double AnomalyMastery => Stats[Affix.AnomalyMastery];
    public double AnomalyProficiency => Stats[Affix.AnomalyProficiency];
    public double EnergyRegen => Stats[Affix.EnergyRegen];
    public double ElementalDmgBonus => Stats[RelatedElementDmg];
    public double ElementalResPen => Stats[RelatedElementRes];
    public double DmgBonus => Stats[Affix.DmgBonus];
    public double ResPen => Stats[Affix.ResPen];
    public double DazeBonus => Stats[Affix.DazeBonus];
    
#if ENERGY_REQUIREMENT_CHECK
    private double _energy = 60;
    public double Energy {
        get => _energy;
        set => _energy = Math.Clamp(value, 0, 120);
    }
#endif

    public virtual SafeDictionary<Affix, double> CollectStats() {
        var result = new SafeDictionary<Affix, double>();

        var maxHp = MaxHp;
        var atk = Atk;
        var def = Def;
        var pen = Pen;
        var penRatio = PenRatio;
        var critRate = CritRate;
        var critDamage = CritDamage;
        var impact = Impact;
        var anomalyMastery = AnomalyMastery;
        var anomalyProficiency = AnomalyProficiency;
        var energyRegen = EnergyRegen;
        var elemDmg = ElementalDmgBonus;
        var elemResPen = ElementalResPen;
        var dmgBonus = DmgBonus;
        var resPen = ResPen;
        var dazeBonus = DazeBonus;

        Add(Affix.Hp, maxHp);
        Add(Affix.Atk, atk);
        Add(Affix.Def, def);
        Add(Affix.Pen, pen);
        Add(Affix.PenRatio, penRatio);
        Add(Affix.CritRate, critRate);
        Add(Affix.CritDamage, critDamage);
        Add(Affix.Impact, impact);
        Add(Affix.AnomalyMastery, anomalyMastery);
        Add(Affix.AnomalyProficiency, anomalyProficiency);
        Add(Affix.EnergyRegen, energyRegen);
        Add(RelatedElementDmg, elemDmg);
        Add(RelatedElementRes, elemResPen);
        Add(Affix.DmgBonus, dmgBonus);
        Add(Affix.ResPen, resPen);
        Add(Affix.DazeBonus, dazeBonus);
    
        return result;

        void Add(Affix affix, double value) { if (value != 0) result[affix] = value; }
    }

    #endregion

    public virtual void RegisterHooks(Context ctx) { }
    
    /// <summary>
    /// Applies the agent's passive to the team
    /// </summary>
    /// <param name="team">Current team, including current agent</param>
    /// <returns>Collection of stats</returns>
    public virtual IEnumerable<Stat> ApplyTeamPassive(List<Agent> team) => [];

    /// <summary>
    /// Applies agent's passive to themselves
    /// </summary>
    public virtual void ApplyPassive() { }

    protected virtual double GetBaseDamage(double scale) => scale / 100 * Atk;

    protected virtual double GetSheerMultiplier() => 1;
    
    /// <summary>
    /// Applies agent's ability's passive
    /// </summary>
    /// <param name="ability">Ability name</param>
    /// <returns><see cref="Stat"/> or null if ability has none</returns>
    public virtual Stat? ApplyAbilityPassive(Ability ability) => null;

    /// <summary>
    /// Calculates standard damage for a given agent and skill
    /// </summary>
    /// <param name="ctx">Current context</param>
    /// <param name="ability">Current skill</param>
    /// <returns><see cref="AgentAction"/> with calculated damage</returns>
    public virtual IEnumerable<AgentAction> GetActionDamage(Context ctx, Ability ability) {
        var data = Skills[ability.Name];
        var attribute = data.Scales[ability.Scale].Element ?? Element;
        var relatedAffixDmg   = Helpers.GetRelatedAffixDmg(attribute);
        var relatedAffixSheer = Helpers.GetRelatedSheerDmg(attribute);
        var relatedAffixRes   = Helpers.GetRelatedAffixRes(attribute);

        ctx.Events.ActionExecuted(ctx, new (this, ability));
        
#if ENERGY_REQUIREMENT_CHECK
        // Energy requirement check
        // ExSpecial has negative energy (using energy)
        // everything else has positive (accumulating energy)
        var multiplier = data.Scales[scale];
        if (Energy + multiplier.Energy < 0) {
            throw new InvalidOperationException($"Agent does not have enough energy to perform {skill} at scale {scale + 1}. " +
                                                $"Required: {Math.Abs(multiplier.Energy)}, current: {Energy}");
        }
        Energy += multiplier.Energy;
#endif

        var tag = data.Tag;

        // Apply ability passive if present. Ability passives are still one-off,
        // so fold them into local accumulators rather than the stat set.
        var abilityDmgBonus = new SafeDictionary<Affix, double>();
        var abilityPassive = ApplyAbilityPassive(ability);
        if (abilityPassive is { } passive) {
            abilityDmgBonus[passive.Affix] += passive.Value;
        }

        // Process anomalies
        var buildup = GetAnomalyBuildup(ability);
        ctx.Enemy.AddAnomalyBuildup(ctx, this, buildup);

        // Calculate damage according to formula
        var baseDmgAttacker = GetBaseDamage(data.Scales[ability.Scale].Damage);
        var dmgBonusMultiplier = 1 + Stats[relatedAffixDmg].For(tag) + Stats[Affix.DmgBonus].For(tag)
                                 + abilityDmgBonus[relatedAffixDmg] + abilityDmgBonus[Affix.DmgBonus]
                                 + data.Affixes[relatedAffixDmg] + data.Affixes[Affix.DmgBonus];
        var critMultiplier = 1 + Math.Min(Stats[Affix.CritRate].For(tag) + abilityDmgBonus[Affix.CritRate] + data.Affixes[Affix.CritRate], 1)
            * (Stats[Affix.CritDamage].For(tag) + abilityDmgBonus[Affix.CritDamage] + data.Affixes[Affix.CritDamage]);
        var resMultiplier = 1 + Stats[relatedAffixRes].For(tag) + Stats[Affix.ResPen].For(tag)
                            + abilityDmgBonus[relatedAffixRes] + abilityDmgBonus[Affix.ResPen]
                            + data.Affixes[relatedAffixRes] + data.Affixes[Affix.ResPen];

        var enemyDefenseMultiplier = Speciality is Speciality.Rupture 
            ? 1 
            : ctx.Enemy.GetDefenseMultiplier(PenRatio, Pen);

        var sheerMultiplier = Speciality is Speciality.Rupture 
            ? 1 + GetSheerMultiplier() + Stats[Affix.SheerForceBonus].For(tag) + Stats[relatedAffixSheer].For(tag)
              + data.Affixes[Affix.SheerForceBonus] + data.Affixes[relatedAffixSheer] 
            : 1;
        
        var total = baseDmgAttacker * dmgBonusMultiplier * critMultiplier * enemyDefenseMultiplier
                    * resMultiplier * sheerMultiplier * DamageTakenMultiplier * ctx.Enemy.StunMultiplier;

        return [new(
            Id, 
            $"{ability.Name} {(ability.Scale == 0 && data.Scales.Count == 1 ? "" : ability.Scale + 1)}".Trim(), 
            data.Tag, 
            total,
            GetDaze(ability)
        )];
    }

    public virtual double GetDaze(Ability ability) {
        var data = Skills[ability.Name];

        var tagDazeBonus = 1.0 + Stats[Affix.DazeBonus].Tagged(data.Tag);
        
        var abilityPassive = ApplyAbilityPassive(ability);
        if (abilityPassive is { Affix: Affix.DazeBonus } passive) {
            tagDazeBonus += passive.Value;
        }
        
        var dazeScale = data.Scales[ability.Scale].Daze / 100;
        var dazeIncrease = 0.0 + tagDazeBonus;
        const double dazeReduction = 0.0;
        const double dazeRes = 0.0;
        const double dazeTakenIncrease = 0.0;
        const double dazeTakenReduction = 0.0;
        return Impact * dazeScale * (1 + dazeIncrease - dazeReduction)
               * (1 - dazeRes) * (1 + dazeTakenIncrease - dazeTakenReduction);
    }

    public virtual double GetDisorderDaze(Enemy enemy) {
        if (enemy.AfflictedAnomaly is not { } anomaly) return 0;

        const double dazeMv = 2;
        const double dazeLevelMultiplier = 1 + 0.0075 * 60; // 60 - character level
        var dazeMultiplier = 1 + Stats[Affix.DazeBonus].For(SkillTag.AttributeAnomaly)
                               + anomaly.Stats[Affix.DazeBonus];
        const double dazeTakenMultiplier = 1;
        const double dazeRes = 1;
        return dazeMv * dazeLevelMultiplier * Impact * dazeRes * dazeMultiplier * dazeTakenMultiplier;
    }
    
    public double GetAnomalyBuildup(Ability ability) {
        var data = Skills[ability.Name];
        var baseBuildup = data.Scales[ability.Scale].AnomalyBuildup;
        if (baseBuildup == 0) return 0;

        var amBonus = AnomalyMastery / 100;
        var amBuildupBonus = 1 + Stats[Affix.AnomalyBuildupBonus].For(ability.Tag)
                               + data.Affixes[Affix.AnomalyBuildupBonus];

        const double amBuildupRes = 1d;

        return baseBuildup * amBonus * amBuildupBonus * amBuildupRes;
    }
    
    public virtual AgentAction GetAnomalyDamage(Context ctx, Element element, bool skipEvents = false) {
        // Agents can override default anomalies
        if (!Anomalies.TryGetValue(element, out var data)) {
            data = Anomaly.GetAnomalyByElement(element)!;
        }
        
        // Prevent Abloom from causing a stack overflow by recursion
        if (!skipEvents)
            ctx.Events.ActionExecuted(ctx, new(this, new(SkillTag.AttributeAnomaly, data.ToString())));
        
        // Some characters can make anomalies crit
        // ...for the entire team, apparently...
        double anomalyCritMultiplier = ctx.AnomalyCritMultiplier;

        var anomalyProficiency = element != Element.None 
            ? AnomalyProficiency 
            : ctx.Enemy.AfflictedAnomaly?.Stats[Affix.AnomalyProficiency] ?? 0;

        // Calculate anomaly damage according to formula
        var anomalyBaseDmg = element != Element.None 
            ? data.Scale / 100 * Atk 
            : GetDisorderBaseMultiplier(ctx.Enemy.AfflictedAnomaly!.Element, ctx.Enemy.AfflictedAnomaly?.Stats[Affix.Atk] ?? 0);
        
        var anomalyProficiencyMultiplier = anomalyProficiency / 100;
        const double anomalyLevelMultiplier = 2;
        var dmgBonusMultiplier = element is Element.None ? 1 : 1 + ElementalDmgBonus
                                                                 + Stats[Affix.DmgBonus].For(SkillTag.AttributeAnomaly)
                                                                 + Stats[Affix.AnomalyDmgBonus].Value;
        var resMultiplier = element != Element.None ? 1 + ElementalResPen + ResPen : 1;

        var disorderElementalMultiplier = 1d;
        var disorderElementalRes = 1d;
        if (element is Element.None && ctx.Enemy.AfflictedAnomaly is { } enemyAnomaly) {
            var disorderElementalDmgBonus = Helpers.GetRelatedAffixDmg(enemyAnomaly.Element);
            var disorderElementalResPen = Helpers.GetRelatedAffixRes(enemyAnomaly.Element);
            disorderElementalMultiplier += enemyAnomaly.Stats[disorderElementalDmgBonus];
            disorderElementalRes += enemyAnomaly.Stats[disorderElementalResPen];

            dmgBonusMultiplier += Stats[Affix.DisorderDmgBonus].Value + enemyAnomaly.Stats[Affix.DmgBonus];
            resMultiplier += enemyAnomaly.Stats[Affix.ResPen];
        }
        
        var total = anomalyBaseDmg * anomalyProficiencyMultiplier * anomalyCritMultiplier * anomalyLevelMultiplier
                    * dmgBonusMultiplier 
                    * ctx.Enemy.GetDefenseMultiplier(ctx.Enemy.AfflictedAnomaly?.Stats[Affix.PenRatio] ?? PenRatio,
                        ctx.Enemy.AfflictedAnomaly?.Stats[Affix.Pen] ?? Pen) 
                    * resMultiplier * 
                    (element is Element.None ? ctx.Enemy.StunMultiplier : 1) * disorderElementalMultiplier * disorderElementalRes;

        return new(
            data.AgentId != 0 ? data.AgentId : Id,
            data.ToString(),
            SkillTag.AttributeAnomaly,
            total,
            element is Element.None ? GetDisorderDaze(ctx.Enemy) : 0
        );
    }

    protected double GetDisorderTimeMultiplier(Element element, Func<double, double>? mvReducer = null) {
        var duration = (element is Element.Frost ? 20 : 10) - 3;

        mvReducer ??= prev => prev;
        
        return element switch {
            Element.Fire => mvReducer(4.5) + duration / 0.5 * 0.5,
            Element.Electric => mvReducer(4.5) + duration * 1.25,
            Element.Ice => mvReducer(4.5) + duration * 0.075,
            Element.Frost => mvReducer(6) + duration * 0.75,
            Element.Physical => mvReducer(4.5) + duration * 0.075,
            Element.Ether or Element.AuricInk => mvReducer(4.5) + duration / 0.5 * 0.625,
            _ => 0
        };
    }
    
    protected virtual double GetDisorderBaseMultiplier(Element element, double attack, Func<double, double>? mvReducer = null) {
        if (element is Element.None) {
            throw new ArgumentException("Disorder cannot trigger itself", nameof(element));
        }

        return GetDisorderTimeMultiplier(element, mvReducer) * attack;
    }

    #region Operators

    public static bool operator ==(Agent left, Agent right) {
        return left.Id == right.Id;
    }

    public static bool operator !=(Agent left, Agent right) {
        return !(left == right);
    }

    public override bool Equals(object? obj) => obj is Agent agent && this == agent;
    
    public override int GetHashCode() => Id.GetHashCode();

    #endregion
}