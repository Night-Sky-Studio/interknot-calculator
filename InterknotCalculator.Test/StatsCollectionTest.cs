using InterknotCalculator.Core.Classes;
using InterknotCalculator.Core.Classes.Agents;
using InterknotCalculator.Core.Enums;

namespace InterknotCalculator.Test;

[TestFixture]
public class StatsCollectionTest {
    private void CheckStats(Dictionary<Affix, double> expected, Dictionary<Affix, double> actual) {
        foreach (var (key, value) in expected) {
            Console.WriteLine($"Testing {key}");
            var actualValue = actual[key] > 2 ? Math.Floor(actual[key]) : actual[key];
            Assert.That(actualValue, Is.EqualTo(value).Within(key is Affix.Hp ? 1 : 0.001), $"Key: {key}");
        }
    }
    
    [Test]
    public void MiyabiStatsTest() {
        var reference = new SafeDictionary<Affix, double> {
            [Affix.Hp] = 11_129,
            [Affix.Atk] = 2_899,
            [Affix.Def] = 790,
            [Affix.Impact] = 86,
            [Affix.CritRate] = 0.706,
            [Affix.CritDamage] = 1.668,
            [Affix.EnergyRegen] = 1.2,
            [Affix.AnomalyProficiency] = 247,
            [Affix.AnomalyMastery] = 116,
            [Affix.Pen] = 36,
            [Affix.PenRatio] = 0.24
        };
        
        var miyabi = new Miyabi();
        miyabi.SetWeapon(WeaponId.HailstormShrine);
        miyabi.SetDriveDiscs([
            new(DriveDiscSetId.WoodpeckerElectro, 1, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Hp, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 3)
                ]),
            new(DriveDiscSetId.BranchBladeSong, 2, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Atk, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.AnomalyProficiency),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.Pen)
                ]),
            new(DriveDiscSetId.WoodpeckerElectro, 3, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Def, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate, 4),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage),
                    Stat.SubStat.Get(Rarity.S, Affix.Atk, 2)
                ]),
            new(DriveDiscSetId.BranchBladeSong, 4, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.CritDamage, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio),
                    Stat.SubStat.Get(Rarity.S, Affix.Pen, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.Atk, 2)
                ]),
            new(DriveDiscSetId.BranchBladeSong, 5, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.PenRatio, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 4),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio),
                    Stat.SubStat.Get(Rarity.S, Affix.Hp)
                ]),
            new(DriveDiscSetId.BranchBladeSong, 6, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.AtkRatio, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.Hp, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.Atk, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate)
                ])
        ]);
        
        CheckStats(reference, miyabi.BaseStats);
    }

    [Test]
    public void JaneStatsTest() {
        var reference = new SafeDictionary<Affix, double> {
            [Affix.Hp] = 11_837,
            [Affix.Atk] = 2_673,
            [Affix.Def] = 965,
            [Affix.Impact] = 86,
            [Affix.CritRate] = 0.098,
            [Affix.CritDamage] = 0.596,
            [Affix.EnergyRegen] = 1.2,
            [Affix.AnomalyProficiency] = 425,
            [Affix.AnomalyMastery] = 192,
            [Affix.Pen] = 36,
            [Affix.PhysicalDmgBonus] = 0.4
        };

        var jane = new JaneDoe();
        jane.SetWeapon(WeaponId.SharpenedStinger);
        jane.SetDriveDiscs([
            new(DriveDiscSetId.FreedomBlues, 1, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Hp, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.AnomalyProficiency, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.Pen, 2)
                ]),
            new(DriveDiscSetId.FreedomBlues, 2, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Atk, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.AnomalyProficiency, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate)
                ]),
            new(DriveDiscSetId.FangedMetal, 3, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Def, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.Hp),
                    Stat.SubStat.Get(Rarity.S, Affix.DefRatio, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 4),
                    Stat.SubStat.Get(Rarity.S, Affix.AnomalyProficiency, 2)
                ]),
            new(DriveDiscSetId.FangedMetal, 4, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.AnomalyProficiency, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate),
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.Hp, 3)
                ]),
            new(DriveDiscSetId.FangedMetal, 5, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.PhysicalDmgBonus, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.AnomalyProficiency, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.Def, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 2)
                ]),
            new(DriveDiscSetId.FangedMetal, 6, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.AnomalyMasteryRatio, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.AnomalyProficiency, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.Pen, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.DefRatio, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio, 2)
                ])
        ]);

        CheckStats(reference, jane.BaseStats);
    }

    [Test]
    public void YixuanStatsTest() {
        var reference = new SafeDictionary<Affix, double> {
            [Affix.Hp] = 19_114,
            [Affix.Atk] = 2_211,
            [Affix.Def] = 685,
            [Affix.Impact] = 93,
            [Affix.CritRate] = 0.458,
            [Affix.CritDamage] = 1.764,
            [Affix.AnomalyProficiency] = 90,
            [Affix.AnomalyMastery] = 92,
            [Affix.Pen] = 9,
            [Affix.Sheer] = 2_574,
            [Affix.EtherDmgBonus] = 0.3
        };

        var yixuan = new Yixuan();
        yixuan.SetWeapon(WeaponId.QingmingBirdcage);
        yixuan.SetDriveDiscs([
            new(DriveDiscSetId.YunkuiTales, 1, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Hp, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.Def)
                ]),
            new(DriveDiscSetId.BranchBladeSong, 2, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Atk, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 4),
                    Stat.SubStat.Get(Rarity.S, Affix.Hp, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.Def)
                ]),
            new(DriveDiscSetId.YunkuiTales, 3, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.Def, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio)
                ]),
            new(DriveDiscSetId.BranchBladeSong, 4, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.CritDamage, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.Hp, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.HpRatio, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate, 4),
                    Stat.SubStat.Get(Rarity.S, Affix.Pen)
                ]),
            new(DriveDiscSetId.YunkuiTales, 5, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.EtherDmgBonus, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.Def, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.Hp, 2)
                ]),
            new(DriveDiscSetId.YunkuiTales, 6, Rarity.S,
                Stat.MainStat.Get(Rarity.S, Affix.HpRatio, 15),
                [
                    Stat.SubStat.Get(Rarity.S, Affix.Atk, 2),
                    Stat.SubStat.Get(Rarity.S, Affix.CritRate),
                    Stat.SubStat.Get(Rarity.S, Affix.CritDamage, 3),
                    Stat.SubStat.Get(Rarity.S, Affix.AtkRatio, 2)
                ])
        ]);

        CheckStats(reference, yixuan.BaseStats);
    }

}