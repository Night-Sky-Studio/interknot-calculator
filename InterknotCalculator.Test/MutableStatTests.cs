using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Modifiers;
using InterknotCalculator.Core.Classes.Server;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test;

[TestFixture]
public class MutableStatTests {
    [Test]
    public void ValueFoldingTest() {
        var hp = new MutableStat(7673);
        
        hp += new Modifier(new("disc-1", AgentId.Ellen), 2200);
        hp += new Modifier(new("disc-4-2", AgentId.Ellen), 0.06, ModifierType.Multiplicative);
        hp += new Modifier(new("disc-3-3", AgentId.Ellen), 0.03, ModifierType.Multiplicative);
        hp += new Modifier(new("disc-6-1", AgentId.Ellen), 224);
        
        Assert.That(hp.Value, Is.InRange(10787, 10788));
        hp.RemoveKey(new("disc-6-1", AgentId.Ellen));
        
        Assert.That(hp.Value, Is.InRange(10563, 10564));
        hp.RemoveKey(new("disc-3-3", AgentId.Ellen));

        Assert.That(hp.Value, Is.InRange(10333, 10334));
    }

    [Test]
    public void BaseModifiersScaleWithRatios() {
        // A weapon's main stat scales with ATK%; a flat passive lands on top of it.
        var atk = new MutableStat(863);

        atk += new Modifier(new("weapon-main", WeaponId.DeepSeaVisitor), 713, ModifierType.Base);
        atk += new Modifier(new("disc-1", AgentId.Ellen), 0.3, ModifierType.Multiplicative);
        atk += new Modifier(new("passive", AgentId.Ellen), 100);

        Assert.That(atk.Value, Is.EqualTo((863 + 713) * 1.3 + 100).Within(1e-9));
    }

    [Test]
    public void TaggedModifiersOnlyCountForTheirAbilities() {
        var dmgBonus = new MutableStat();

        dmgBonus += new Modifier(new("disc-set-full", DriveDiscSetId.FangedMetal), 0.35);
        dmgBonus += new Modifier(new("disc-set-full", DriveDiscSetId.PolarMetal), 0.4,
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
        var key = new ModifierKey("core", AgentId.Ellen);

        critRate.Add(new(key, 0.12));

        Assert.Multiple(() => {
            Assert.That(critRate.Has(key), Is.True);
            Assert.That(() => critRate.Add(new(key, 0.12)), Throws.ArgumentException);
            Assert.That(critRate.Value, Is.EqualTo(0.17).Within(1e-9));
        });

        // ...but it can once it has been removed.
        critRate.RemoveKey(key);
        Assert.That(critRate.Has(key), Is.False);
        Assert.That(() => critRate.Add(new(key, 0.12)), Throws.Nothing);
    }

    [Test]
    public void ClearKeepsBaseValue() {
        var impact = new MutableStat(93);
        impact += new Modifier(new("passive", AgentId.Lycaon), 50);
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