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
    protected readonly record struct Hit(Context Context, Ability Ability, Element Element, Scale MotionValue);
    
    private const double DamageTakenMultiplier = 1;

    #region Information
    public uint Id { get; } = id;
    public Speciality Speciality { get; set; }
    public Element Element { get; set; }
    public Rarity Rarity { get; set; }
    public Faction Faction { get; set; }
    #endregion
    
    #region Collections
    private StatsDictionary Stats { get; } = new();
    public Dictionary<Element, Anomaly> Anomalies { get; set; } = new();
    public Dictionary<string, Skill> Skills { get; set; } = new();
    public Dictionary<string, IEnumerable<string>> Macros { get; set; } = new();
    #endregion

    #region Equipment

    public Weapon? Weapon { get; protected set; }
    public DriveDisc[] DriveDiscs { get; private set; } = [];
    public List<DriveDiscSet> PartialSets { get; } = [];
    public List<DriveDiscSet> FullSets { get; } = [];

    public void SetWeapon(uint weaponId) {
        RemoveWeaponStats();
        Weapon = WeaponRegistry.CreateInstance(weaponId);
        AddWeaponStats();
    }
    protected void RemoveWeaponStats() {
        if (Weapon is null) 
            return;
        Weapon = null;
        Stats.RemoveAllModifiers(m => m.Key.ComponentStartsWith("Weapon:"));
    }
    protected void AddWeaponStats(bool passiveOnly = false) {
        if (Weapon is not { } w)
            return;
        if (!passiveOnly) {
            Stats[w.MainStat.Affix.Flat()].Add(ModifierKey.Agent(Id) + ModifierKey.Weapon(w.Id) + ModifierKey.MainStat(),
                w.MainStat.Value, ModifierType.Base);
            Stats[w.SecondaryStat.Affix.Flat()].Add(ModifierKey.Agent(Id) + ModifierKey.Weapon(w.Id) + ModifierKey.SecondaryStat(),
                w.SecondaryStat);
        }
        if (w.Speciality != Speciality) 
            return;
        foreach (var passive in w.Passive) {
            Stats[passive.Affix.Flat()].Add(ModifierKey.Agent(Id) + ModifierKey.Weapon(w.Id) + ModifierKey.Passive(), 
                passive, true);
        }
    }

    public void SetDriveDiscs(DriveDisc[] discs) {
        RemoveDiscsStats();
        DriveDiscs = discs;
        AddDiscsStats();
    }
    protected void RemoveDiscsStats() {
        if (DriveDiscs.Length == 0 && FullSets.Count == 0 && PartialSets.Count == 0)
            return;
        FullSets.Clear();
        PartialSets.Clear();
        Stats.RemoveAllModifiers(m => m.Key.ComponentStartsWith("Disc:"));
    }
    protected void AddDiscsStats(bool passiveOnly = false) {
        if (!passiveOnly) {
            var setCounts = new SafeDictionary<uint, int>();

            // First pass: count set membership and resolve which sets are partial/full.
            foreach (var disc in DriveDiscs) {
                setCounts[disc.SetId] += 1;
            }

            foreach (var (setId, count) in setCounts) {
                if (count >= 2) PartialSets.Add(DriveDiscSetRegistry.CreateInstance(setId));
                if (count >= 4) FullSets.Add(DriveDiscSetRegistry.CreateInstance(setId));
            }

            // Second pass: apply each disc's main stat and substats.
            foreach (var disc in DriveDiscs) {
                Stats[disc.MainStat.Affix.Flat()].Add(ModifierKey.Agent(Id) + ModifierKey.Disc(disc.Slot) +
                    ModifierKey.Stat(disc.MainStat.Affix, disc.MainStat.Level), disc.MainStat);
                foreach (var subStat in disc.SubStats) {
                    Stats[subStat.Affix.Flat()].Add(ModifierKey.Agent(Id) + ModifierKey.Disc(disc.Slot) +
                        ModifierKey.Stat(subStat.Affix, subStat.Level), subStat);
                }
            }
        }
        
        foreach (var set in PartialSets) {
            foreach (var bonus in set.PartialBonus) {
                Stats[bonus.Affix.Flat()].Add(ModifierKey.Agent(Id) + ModifierKey.DiscSet(set.Id), bonus);
            }
        }

        foreach (var set in FullSets) {
            foreach (var bonus in set.FullBonus) {
                Stats[bonus.Affix.Flat()].Add(ModifierKey.Agent(Id) + ModifierKey.DiscSet(set.Id, true),
                    bonus, true);
            }
        }
    }
    #endregion
    
    #region Stats
    public Affix RelatedElementDmg => Helpers.GetRelatedAffixDmg(Element);
    public Affix RelatedElementRes => Helpers.GetRelatedAffixRes(Element);

    public MutableStat MaxHp => Stats[Affix.Hp];
    public double Hp {
        get => Math.Clamp(field, 0, MaxHp); 
        set => field = Math.Clamp(value, 0, MaxHp);
    }
    public MutableStat Atk => Stats[Affix.Atk];
    public MutableStat Def => Stats[Affix.Def];
    public MutableStat Pen => Stats[Affix.Pen];
    public MutableStat PenRatio => Stats[Affix.PenRatio];
    public MutableStat DefPen => Stats[Affix.DefPen];
    public MutableStat CritRate => Stats[Affix.CritRate];
    public MutableStat CritDamage => Stats[Affix.CritDamage];
    public MutableStat Impact => Stats[Affix.Impact];
    public MutableStat AnomalyMastery => Stats[Affix.AnomalyMastery];
    public MutableStat AnomalyProficiency => Stats[Affix.AnomalyProficiency];
    public MutableStat EnergyRegen => Stats[Affix.EnergyRegen];
    public MutableStat ElementalDmgBonus => Stats[RelatedElementDmg];
    public MutableStat ElementalResPen => Stats[RelatedElementRes];
    public MutableStat DmgBonus => Stats[Affix.DmgBonus];
    public MutableStat ResPen => Stats[Affix.ResPen];
    public MutableStat DazeBonus => Stats[Affix.DazeBonus];
    public MutableStat AnomalyDmgBonus => Stats[Affix.AnomalyDmgBonus];
    public MutableStat DisorderDmgBonus => Stats[Affix.DisorderDmgBonus];
    public MutableStat AnomalyBuildupBonus => Stats[Affix.AnomalyBuildupBonus];
    
    public MutableStat GetStat(Affix affix) => Stats[affix];
    
#if ENERGY_REQUIREMENT_CHECK
    private double _energy = 60;
    public double Energy {
        get => _energy;
        set => _energy = Math.Clamp(value, 0, 120);
    }
#endif

    protected void InitializeStats(Dictionary<Affix, double> stats) {
        foreach (var (affix, value) in stats) {
            Stats[affix] = new(value);
        }
    }
    
    public virtual SafeDictionary<Affix, double> CollectStats(bool initial = false) {
        var result = new SafeDictionary<Affix, double>();

        var maxHp = MaxHp;
        var atk = Atk;
        var def = Def;
        var pen = Pen;
        var penRatio = PenRatio;
        var defPen = DefPen;
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

        Add(Affix.Hp, initial ? maxHp.InitialValue : maxHp);
        Add(Affix.Atk, initial ? atk.InitialValue : atk);
        Add(Affix.Def, initial ? def.InitialValue : def);
        Add(Affix.Pen, initial ? pen.InitialValue : pen);
        Add(Affix.PenRatio, initial ? penRatio.InitialValue : penRatio);
        Add(Affix.DefPen, initial ? defPen.InitialValue : defPen);
        Add(Affix.CritRate, initial ? critRate.InitialValue : critRate);
        Add(Affix.CritDamage, initial ? critDamage.InitialValue : critDamage);
        Add(Affix.Impact, initial ? impact.InitialValue : impact);
        Add(Affix.AnomalyMastery, initial ? anomalyMastery.InitialValue : anomalyMastery);
        Add(Affix.AnomalyProficiency, initial ? anomalyProficiency.InitialValue : anomalyProficiency);
        Add(Affix.EnergyRegen, initial ? energyRegen.InitialValue : energyRegen);
        Add(RelatedElementDmg, initial ? elemDmg.InitialValue : elemDmg);
        Add(RelatedElementRes, initial ? elemResPen.InitialValue : elemResPen);
        Add(Affix.DmgBonus, initial ? dmgBonus.InitialValue : dmgBonus);
        Add(Affix.ResPen, initial ? resPen.InitialValue : resPen);
        Add(Affix.DazeBonus, initial ? dazeBonus.InitialValue : dazeBonus);
    
        return result;

        void Add(Affix affix, double value) { if (value != 0) result[affix] = value; }
    }

    #endregion

    public virtual void RegisterHooks(Context ctx) { }
    
    protected virtual double GetBaseDamage(in Hit hit) => hit.MotionValue.Damage / 100 * Atk;
    
    protected virtual double GetDmgBonusMultiplier(in Hit hit) => 
        1 + Stats[Helpers.GetRelatedAffixDmg(hit.Element)].For(hit.Ability) 
          + Stats[Affix.DmgBonus].For(hit.Ability);
    
    protected virtual double GetCritMultiplier(in Hit hit) =>
        1 + Math.Min(Stats[Affix.CritRate].For(hit.Ability), 1) * Stats[Affix.CritDamage].For(hit.Ability);

    protected virtual double GetResMultiplier(in Hit hit) => 
        1 + Stats[Helpers.GetRelatedAffixRes(hit.Element)].For(hit.Ability) 
          + Stats[Affix.ResPen].For(hit.Ability);

    protected virtual double GetEnemyDefenseMultiplier(in Hit hit) =>
        hit.Context.Enemy.GetDefenseMultiplier(Stats[Affix.DefPen].For(hit.Ability), PenRatio, Pen);

    protected virtual double GetSpecialityMultiplier(in Hit hit) => 1;

    protected virtual Skill GetSkill(in Ability ability) => Skills[ability.Name];
    
    protected Hit GetHit(Context ctx, Ability ability) {
        var skill = GetSkill(ability);
        var element = skill.Scales[ability.Scale].Element ?? Element;
        var motionValue = skill.Scales[ability.Scale];

        return new(ctx, ability, element, motionValue);
    }
    
    /// <summary>
    /// Calculates standard damage for a given agent and skill
    /// </summary>
    /// <param name="ctx">Current context</param>
    /// <param name="ability">Current skill</param>
    /// <returns><see cref="AgentAction"/> with calculated damage</returns>
    public virtual AgentAction GetActionDamage(Context ctx, Ability ability) {
        var hit = GetHit(ctx, ability);
        
        ctx.Events.ActionExecuting(ctx, new (this, ability));
        
#if ENERGY_REQUIREMENT_CHECK
        // Energy requirement check
        // ExSpecial has negative energy (using energy)
        // everything else has positive (accumulating energy)
        var multiplier = hit.MotionValue;
        if (Energy + multiplier.Energy < 0) {
            throw new InvalidOperationException($"Agent does not have enough energy to perform {ability.Name} at scale {ability.Scale + 1}. " +
                                                $"Required: {Math.Abs(multiplier.Energy)}, current: {Energy}");
        }
        Energy += multiplier.Energy;
#endif

        // Process anomalies
        var buildup = GetAnomalyBuildup(ctx, ability);
        ctx.Enemy.AddAnomalyBuildup(ctx, this, buildup);

        // Calculate damage according to formula
        var baseDmgAttacker = GetBaseDamage(hit);
        var dmgBonusMultiplier = GetDmgBonusMultiplier(hit);
        var critMultiplier = GetCritMultiplier(hit);
        var resMultiplier = GetResMultiplier(hit);
        var enemyDefenseMultiplier = GetEnemyDefenseMultiplier(hit);
        var specialityMultiplier = GetSpecialityMultiplier(hit);
        
        var total = baseDmgAttacker * dmgBonusMultiplier * critMultiplier * enemyDefenseMultiplier
                    * resMultiplier * specialityMultiplier * DamageTakenMultiplier * ctx.Enemy.StunMultiplier;

        var action = new AgentAction(
            Id,
            GetSkill(ability).Scales.Count > 1 ? $"{ability.Name} {ability.Scale + 1}" : ability.Name,
            ability.Tag,
            total,
            GetDaze(ctx, ability)
        );
        
        ctx.Events.ActionExecuted(ctx, new(this, ability, action));
        
        return action;
    }

    public virtual double GetDaze(Context ctx, Ability ability) {
        var daze = GetHit(ctx, ability);
        
        var dazeScale = daze.MotionValue.Daze / 100;
        var dazeIncrease = Stats[Affix.DazeBonus].For(ability);
        
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
    
    public double GetAnomalyBuildup(Context ctx, Ability ability) {
        var hit = GetHit(ctx, ability);
        var baseBuildup = hit.MotionValue.AnomalyBuildup;
        if (baseBuildup == 0) return 0;

        var amBonus = AnomalyMastery / 100;
        var amBuildupBonus = 1 + Stats[Affix.AnomalyBuildupBonus].For(ability);

        const double amBuildupRes = 1d;

        return baseBuildup * amBonus * amBuildupBonus * amBuildupRes;
    }
    
    public virtual AgentAction GetAnomalyDamage(Context ctx, Element element, bool skipEvents = false) {
        // Agents can override default anomalies
        if (!Anomalies.TryGetValue(element, out var data)) {
            data = Anomaly.GetDefaultAnomalyByElement(element);
        }
        
        // Prevent Abloom from causing a stack overflow by recursion
        if (!skipEvents)
            ctx.Events.ActionExecuting(ctx, new(this, new(SkillTag.AttributeAnomaly, data.ToString())));
        
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
                    * ctx.Enemy.GetDefenseMultiplier(
                        ctx.Enemy.AfflictedAnomaly?.Stats[Affix.DefPen] ?? DefPen,
                        ctx.Enemy.AfflictedAnomaly?.Stats[Affix.PenRatio] ?? PenRatio,
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