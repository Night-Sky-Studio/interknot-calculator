using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test;

[TestFixture]
public class MutableStatTests {
    [Test]
    public void ValueFoldingTest() {
        var hp = new MutableStat(7673);
        
        hp.Add(ModifierKey.Agent(AgentId.Ellen) + ModifierKey.Disc(1), 2200, ModifierType.Flat);
        hp.Add(ModifierKey.Agent(AgentId.Ellen) + ModifierKey.Disc(4) + ModifierKey.Stat(Affix.HpRatio, 2), 
            0.06, ModifierType.Ratio);
        
        var disc33 = hp.Add(ModifierKey.Agent(AgentId.Ellen) + ModifierKey.Disc(3) + ModifierKey.Stat(Affix.HpRatio, 1), 
            0.03, ModifierType.Ratio);
        
        var disc61 = hp.Add(ModifierKey.Agent(AgentId.Ellen) + ModifierKey.Disc(6) + ModifierKey.Stat(Affix.Hp, 1), 
            224, ModifierType.Flat);
        
        Assert.That(hp.Value, Is.EqualTo(10787.57).Within(1e-9));
        hp.Remove(disc61);
        
        Assert.That(hp.Value, Is.EqualTo(10563.57).Within(1e-9));
        hp.Remove(disc33);

        Assert.That(hp.Value, Is.EqualTo(10333.38).Within(1e-9));
    }


    [Test]
    public void NegativeValueFoldingTest() {
        var def = new MutableStat(953);

        def.Subtract(ModifierKey.Agent(AgentId.Nicole) + ModifierKey.CorePassive(), 53, ModifierType.CombatFlat);
        Assert.That(def.Value, Is.EqualTo(900).Within(1e-9));
        
        def.Subtract(ModifierKey.Agent(AgentId.Nicole) + ModifierKey.CorePassive(), 0.4, ModifierType.CombatRatio);
        Assert.That(def.Value, Is.EqualTo(518.8).Within(1e-9));
    }

    [Test]
    public void BaseModifiersScaleWithRatios() {
        // A weapon's main stat scales with ATK%; a flat passive lands on top of it.
        var atk = new MutableStat(863);

        atk.Add(new("weapon-main", WeaponId.DeepSeaVisitor.ToString()), 713, ModifierType.Base);
        atk.Add(new("disc-1", AgentId.Ellen.ToString()), 0.3, ModifierType.Ratio);
        atk.Add(new("passive", AgentId.Ellen.ToString()), 100, ModifierType.CombatFlat);

        Assert.That(atk.Value, Is.EqualTo((863 + 713) * 1.3 + 100).Within(1e-9));
    }

    [Test]
    public void TaggedModifiersOnlyCountForTheirAbilities() {
        var dmgBonus = new MutableStat();

        dmgBonus.Add(new("disc-set-full", DriveDiscSetId.FangedMetal.ToString()), 
            0.35, ModifierType.CombatFlat);
        dmgBonus.Add(new("disc-set-full", DriveDiscSetId.PolarMetal.ToString()), 
            0.4, ModifierType.CombatFlat,
            filter: SkillTag.BasicAtk | SkillTag.Dash);

        Assert.Multiple(() => {
            // Unconditional only.
            Assert.That(dmgBonus.Value, Is.EqualTo(0.35).Within(1e-9));
            Assert.That(dmgBonus.For(SkillTag.BasicAtk), Is.EqualTo(0.75).Within(1e-9));
            Assert.That(dmgBonus.For(SkillTag.Dash), Is.EqualTo(0.75).Within(1e-9));
            Assert.That(dmgBonus.For(SkillTag.Ultimate), Is.EqualTo(0.35).Within(1e-9));
            Assert.That(dmgBonus.For(SkillTag.None), Is.EqualTo(0.35).Within(1e-9));
            Assert.That(dmgBonus.For(new Ability(SkillTag.BasicAtk, "any")), Is.EqualTo(0.75).Within(1e-9));
            Assert.That(dmgBonus.For(new Ability(SkillTag.Ultimate, "any")), Is.EqualTo(0.35).Within(1e-9));
        });
    }

    [Test]
    public void TogglingActiveChangesValue() {
        var atk = new MutableStat(1000);
        var buff = atk.Add(new("buff"), 200, ModifierType.CombatFlat);
        Assert.That(atk.Value, Is.EqualTo(1200).Within(1e-9));

        buff.Active = false;
        Assert.Multiple(() => {
            Assert.That(atk.Value, Is.EqualTo(1000).Within(1e-9));
            Assert.That(atk.Modifiers, Has.Count.EqualTo(1));
        });

        buff.Active = true;
        Assert.That(atk.Value, Is.EqualTo(1200).Within(1e-9));
    }

    [Test]
    public void SettingActiveToSameValueDoesNotInvalidate() {
        var atk = new MutableStat(1000);
        var buff = atk.Add(new("buff"), 200, ModifierType.CombatFlat);
        _ = atk.Value;
        var folds = atk.FoldCount;

        buff.Active = true;
        _ = atk.Value;
        Assert.That(atk.FoldCount, Is.EqualTo(folds));

        buff.Active = false;
        _ = atk.Value;
        Assert.That(atk.FoldCount, Is.EqualTo(folds + 1));
    }

    [Test]
    public void DynamicPredicateIsCheckedOnEveryRead() {
        var atk = new MutableStat(1000);
        var condition = false;
        atk.Add(new("dynamic"), 200, ModifierType.CombatFlat, when: () => condition);
        atk.Add(new("dynamic-ratio"), 0.1, ModifierType.Ratio, when: () => condition);

        Assert.Multiple(() => {
            Assert.That(atk.Value, Is.EqualTo(1000).Within(1e-9));
            Assert.That(atk.InitialValue, Is.EqualTo(1000).Within(1e-9));
        });

        condition = true;
        Assert.Multiple(() => {
            Assert.That(atk.Value, Is.EqualTo(1300).Within(1e-9));
            Assert.That(atk.InitialValue, Is.EqualTo(1100).Within(1e-9));
            Assert.That(atk.For(new Ability(SkillTag.BasicAtk, "any")), Is.EqualTo(1300).Within(1e-9));
        });

        condition = false;
        Assert.That(atk.Value, Is.EqualTo(1000).Within(1e-9));
    }

    [Test]
    public void ConditionalModifierOnlyCountsWhenFilterMatches() {
        var dmgBonus = new MutableStat();
        var condition = true;
        dmgBonus.Add(new("named"), 0.3, ModifierType.CombatFlat,
            filter: AbilityFilter.Of("shimotsuki"), when: () => condition);
        dmgBonus.Add(new("scaled"), 0.2, ModifierType.CombatFlat, filter: AbilityFilter.Of("shimotsuki", 2));

        Assert.Multiple(() => {
            Assert.That(dmgBonus.Value, Is.Zero);
            Assert.That(dmgBonus.For(new Ability(SkillTag.BasicAtk, "shimotsuki")), Is.EqualTo(0.3).Within(1e-9));
            Assert.That(dmgBonus.For(new Ability(SkillTag.BasicAtk, "shimotsuki", 2)), Is.EqualTo(0.5).Within(1e-9));
            Assert.That(dmgBonus.For(new Ability(SkillTag.BasicAtk, "kazahana")), Is.Zero);
        });

        condition = false;
        Assert.That(dmgBonus.For(new Ability(SkillTag.BasicAtk, "shimotsuki", 2)), Is.EqualTo(0.2).Within(1e-9));
    }

    [TestCase(ModifierType.Base)]
    [TestCase(ModifierType.Ratio)]
    [TestCase(ModifierType.CombatRatio)]
    public void FilteredModifierMustBeFlat(ModifierType type) {
        var atk = new MutableStat(1000);

        Assert.Multiple(() => {
            Assert.Throws<ArgumentException>(() => atk.Add(new("filtered"), 0.1, type, filter: SkillTag.BasicAtk));
            Assert.That(atk.Modifiers, Is.Empty);
            Assert.That(atk.For(SkillTag.BasicAtk), Is.EqualTo(1000).Within(1e-9));
        });
    }

    [TestCase(ModifierType.Flat)]
    [TestCase(ModifierType.CombatFlat)]
    public void FilteredFlatModifierIsAllowed(ModifierType type) {
        var atk = new MutableStat(1000);

        Assert.DoesNotThrow(() => atk.Add(new("filtered"), 100, type, filter: SkillTag.BasicAtk));
        Assert.That(atk.For(SkillTag.BasicAtk), Is.EqualTo(1100).Within(1e-9));
    }

    [Test]
    public void TagQueryIgnoresNamedFilters() {
        var dmgBonus = new MutableStat();
        dmgBonus.Add(new("tagged"), 0.2, ModifierType.CombatFlat, filter: SkillTag.AttributeAnomaly);
        dmgBonus.Add(new("named"), 0.5, ModifierType.CombatFlat,
            filter: new AbilityFilter(SkillTag.AttributeAnomaly, ["assault"]));

        Assert.Multiple(() => {
            Assert.That(dmgBonus.For(SkillTag.AttributeAnomaly), Is.EqualTo(0.2).Within(1e-9));
            Assert.That(dmgBonus.For(new Ability(SkillTag.AttributeAnomaly, "assault")), Is.EqualTo(0.7).Within(1e-9));
        });
    }

    [Test]
    public void ModifiersWithSameKeyBothCount() {
        var critRate = new MutableStat(0.05);
        var key = new ModifierKey("core", AgentId.Ellen.ToString());

        var first = critRate.Add(key, 0.12, ModifierType.CombatFlat);
        var second = critRate.Add(key, 0.12, ModifierType.CombatFlat);

        Assert.Multiple(() => {
            Assert.That(first, Is.Not.SameAs(second));
            Assert.That(critRate.Modifiers, Has.Count.EqualTo(2));
            Assert.That(critRate.Value, Is.EqualTo(0.29).Within(1e-9));
        });
    }

    [Test]
    public void RemovedModifierNoLongerAffectsValue() {
        var atk = new MutableStat(1000);
        var buff = atk.Add(new("buff"), 200, ModifierType.CombatFlat);
        var dynamic = atk.Add(new("dynamic"), 50, ModifierType.CombatFlat, when: () => true);
        Assert.That(atk.Value, Is.EqualTo(1250).Within(1e-9));

        atk.Remove(buff);
        atk.Remove(dynamic);
        Assert.Multiple(() => {
            Assert.That(atk.Modifiers, Is.Empty);
            Assert.That(atk.Value, Is.EqualTo(1000).Within(1e-9));
        });

        buff.Active = false;
        buff.Active = true;
        Assert.That(atk.Value, Is.EqualTo(1000).Within(1e-9));
    }

    [Test]
    public void StaticValueComesFromCache() {
        var atk = new MutableStat(1000);
        atk.Add(new("static"), 0.1, ModifierType.Ratio);
        atk.Add(new("conditional"), 0.3, ModifierType.CombatFlat, filter: SkillTag.BasicAtk, when: () => true);

        _ = atk.Value;
        var folds = atk.FoldCount;
        for (var i = 0; i < 5; i++) {
            _ = atk.Value;
            _ = atk.InitialValue;
            _ = atk.For(SkillTag.BasicAtk);
        }

        Assert.Multiple(() => {
            Assert.That(atk.FoldCount, Is.EqualTo(folds));
            Assert.That(atk.Value, Is.EqualTo(1100).Within(1e-9));
        });
    }

    [Test]
    public void OwnerIsTheCreatingStat() {
        var atk = new MutableStat(1000);
        var def = new MutableStat(500);

        var atkBuff = atk.Add(new("buff"), 1, ModifierType.Flat);
        var defBuff = def.Subtract(new("debuff"), 1, ModifierType.Flat);

        Assert.Multiple(() => {
            Assert.That(atkBuff.Owner, Is.SameAs(atk));
            Assert.That(defBuff.Owner, Is.SameAs(def));
            Assert.That(defBuff.Value, Is.EqualTo(-1));
            Assert.That(atk.Modifiers, Is.EquivalentTo(new[] { atkBuff }));
        });
    }

    [Test]
    public void ClearKeepsBaseValue() {
        var impact = new MutableStat(93);
        impact.Add(new("passive", AgentId.Lycaon.ToString()), 50, ModifierType.CombatFlat);
        Assert.That(impact.Value, Is.EqualTo(143).Within(1e-9));

        impact.Clear();

        Assert.That(impact.Value, Is.EqualTo(93).Within(1e-9));
        Assert.That(impact.Modifiers, Is.Empty);
    }

    [Test]
    public void DefaultStatIsZero() {
        var stat = new MutableStat();

        Assert.That(stat.Value, Is.Zero);
        Assert.That(stat.BaseValue, Is.Zero);
        Assert.That(stat.Modifiers, Is.Empty);
    }
}
