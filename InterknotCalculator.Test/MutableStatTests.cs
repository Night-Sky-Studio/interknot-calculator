using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Enums;

using Throws = NUnit.Framework.Throws;

namespace InterknotCalculator.Test;

[TestFixture]
public class MutableStatTests {
    [Test]
    public void ValueFoldingTest() {
        var hp = new MutableStat(7673);
        
        hp += new Modifier(ModifierKey.Agent(AgentId.Ellen) + ModifierKey.Disc(1), 2200);
        hp += new Modifier(ModifierKey.Agent(AgentId.Ellen) + ModifierKey.Disc(4) + ModifierKey.Stat(Affix.HpRatio, 2), 0.06, ModifierType.Ratio);
        
        var disc33Key = ModifierKey.Agent(AgentId.Ellen) + ModifierKey.Disc(3) + ModifierKey.Stat(Affix.HpRatio, 1);
        hp += new Modifier(disc33Key, 0.03, ModifierType.Ratio);
        
        var disc61Key = ModifierKey.Agent(AgentId.Ellen) + ModifierKey.Disc(6) + ModifierKey.Stat(Affix.Hp, 1);
        hp += new Modifier(disc61Key, 224);
        
        Assert.That(hp.Value, Is.EqualTo(10787.57).Within(1e-9));
        hp.RemoveKey(disc61Key);
        
        Assert.That(hp.Value, Is.EqualTo(10563.57).Within(1e-9));
        hp.RemoveKey(disc33Key);

        Assert.That(hp.Value, Is.EqualTo(10333.38).Within(1e-9));
    }

    [Test]
    public void BaseModifiersScaleWithRatios() {
        // A weapon's main stat scales with ATK%; a flat passive lands on top of it.
        var atk = new MutableStat(863);

        atk += new Modifier(new("weapon-main", WeaponId.DeepSeaVisitor.ToString()), 713, ModifierType.Base);
        atk += new Modifier(new("disc-1", AgentId.Ellen.ToString()), 0.3, ModifierType.Ratio);
        atk += new Modifier(new("passive", AgentId.Ellen.ToString()), 100);

        Assert.That(atk.Value, Is.EqualTo((863 + 713) * 1.3 + 100).Within(1e-9));
    }

    [Test]
    public void TaggedModifiersOnlyCountForTheirAbilities() {
        var dmgBonus = new MutableStat();

        dmgBonus += new Modifier(new("disc-set-full", DriveDiscSetId.FangedMetal.ToString()), 0.35);
        dmgBonus += new Modifier(new("disc-set-full", DriveDiscSetId.PolarMetal.ToString()), 0.4,
            tags: SkillTag.BasicAtk | SkillTag.Dash);

        Assert.Multiple(() => {
            // Untagged only.
            Assert.That(dmgBonus.Value, Is.EqualTo(0.35).Within(1e-9));
            Assert.That(dmgBonus.Tagged(SkillTag.BasicAtk), Is.EqualTo(0.4).Within(1e-9));
            Assert.That(dmgBonus.Tagged(SkillTag.Dash), Is.EqualTo(0.4).Within(1e-9));
            Assert.That(dmgBonus.Tagged(SkillTag.Ultimate), Is.Zero);
            Assert.That(dmgBonus.Tagged(SkillTag.None), Is.Zero);
            Assert.That(dmgBonus.For(SkillTag.BasicAtk), Is.EqualTo(0.75).Within(1e-9));
            Assert.That(dmgBonus.For(SkillTag.Ultimate), Is.EqualTo(0.35).Within(1e-9));
        });
    }

    [Test]
    public void SameSourceCannotApplyTwice() {
        var critRate = new MutableStat(0.05);
        var key = new ModifierKey("core", AgentId.Ellen.ToString());

        critRate.Add(new(key, 0.12));

        Assert.Multiple(() => {
            Assert.That(critRate.Contains(key), Is.True);
            Assert.That(() => critRate.AddUnique(new(key, 0.12)), Throws.ArgumentException);
            Assert.That(critRate.Value, Is.EqualTo(0.17).Within(1e-9));
        });

        // ...but it can once it has been removed.
        critRate.RemoveKey(key);
        Assert.That(critRate.Contains(key), Is.False);
        Assert.That(() => critRate.Add(new(key, 0.12)), Throws.Nothing);
    }

    [Test]
    public void ClearKeepsBaseValue() {
        var impact = new MutableStat(93);
        impact += new Modifier(new("passive", AgentId.Lycaon.ToString()), 50);
        Assert.That(impact.Value, Is.EqualTo(143).Within(1e-9));

        impact.Clear();

        Assert.That(impact.Value, Is.EqualTo(93).Within(1e-9));
        Assert.That(impact.AppliedModifiers, Is.Empty);
    }

    [Test]
    public void DefaultStatIsZero() {
        var stat = new MutableStat();

        Assert.That(stat.Value, Is.Zero);
        Assert.That(stat.BaseValue, Is.Zero);
        Assert.That(stat.AppliedModifiers, Is.Empty);
    }
}