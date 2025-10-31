using InterknotCalculator.Classes;
using InterknotCalculator.Classes.Agents;
using InterknotCalculator.Enums;

namespace InterknotCalculator.Test;

[TestFixture]
public class StatsCollectionTest: CalculatorTest {
    private void CheckStats(Dictionary<Affix, double> expected, Dictionary<Affix, double> actual) {
        foreach (var (key, value) in expected) {
            Console.WriteLine($"Testing {key}");
            var actualValue = actual[key] > 2 ? Math.Floor(actual[key]) : actual[key];
            Assert.That(actualValue, Is.EqualTo(value).Within(0.001), $"Key: {key}");
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
                Stat.Stats[Affix.Hp],
                [
                    Stat.SubStats[Affix.HpRatio] with { Level = 2 },
                    Stat.SubStats[Affix.CritDamage],
                    Stat.SubStats[Affix.CritRate] with { Level = 3 },
                    Stat.SubStats[Affix.AtkRatio] with { Level = 3 }
                ]),
            new(DriveDiscSetId.BranchBladeSong, 2, Rarity.S,
                Stat.Stats[Affix.Atk],
                [
                    Stat.SubStats[Affix.AnomalyProficiency],
                    Stat.SubStats[Affix.CritDamage] with { Level = 3 },
                    Stat.SubStats[Affix.CritRate] with { Level = 3 },
                    Stat.SubStats[Affix.Pen]
                ]),
            new(DriveDiscSetId.WoodpeckerElectro, 3, Rarity.S,
                Stat.Stats[Affix.Def],
                [
                    Stat.SubStats[Affix.AtkRatio] with { Level = 2 },
                    Stat.SubStats[Affix.CritRate] with { Level = 4 },
                    Stat.SubStats[Affix.CritDamage],
                    Stat.SubStats[Affix.Atk] with { Level = 2 }
                ]),
            new(DriveDiscSetId.BranchBladeSong, 4, Rarity.S,
                Stat.Stats[Affix.CritDamage],
                [
                    Stat.SubStats[Affix.AtkRatio],
                    Stat.SubStats[Affix.Pen] with { Level = 3 },
                    Stat.SubStats[Affix.CritRate] with { Level = 3 },
                    Stat.SubStats[Affix.Atk] with { Level = 2 }
                ]),
            new(DriveDiscSetId.BranchBladeSong, 5, Rarity.S,
                Stat.Stats[Affix.PenRatio],
                [
                    Stat.SubStats[Affix.HpRatio] with { Level = 2 },
                    Stat.SubStats[Affix.CritDamage] with { Level = 4 },
                    Stat.SubStats[Affix.AtkRatio],
                    Stat.SubStats[Affix.Hp]
                ]),
            new(DriveDiscSetId.BranchBladeSong, 6, Rarity.S,
                Stat.Stats[Affix.AtkRatio],
                [
                    Stat.SubStats[Affix.Hp] with { Level = 2 },
                    Stat.SubStats[Affix.CritDamage] with { Level = 2 },
                    Stat.SubStats[Affix.Atk] with { Level = 3 },
                    Stat.SubStats[Affix.CritRate]
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
                Stat.Stats[Affix.Hp],
                [
                    Stat.SubStats[Affix.AnomalyProficiency] with { Level = 2 },
                    Stat.SubStats[Affix.HpRatio] with { Level = 2 },
                    Stat.SubStats[Affix.AtkRatio] with { Level = 3 },
                    Stat.SubStats[Affix.Pen] with { Level = 2 }
                ]),
            new(DriveDiscSetId.FreedomBlues, 2, Rarity.S,
                Stat.Stats[Affix.Atk],
                [
                    Stat.SubStats[Affix.AnomalyProficiency] with { Level = 3 },
                    Stat.SubStats[Affix.HpRatio],
                    Stat.SubStats[Affix.AtkRatio] with { Level = 3 },
                    Stat.SubStats[Affix.CritRate]
                ]),
            new(DriveDiscSetId.FangedMetal, 3, Rarity.S,
                Stat.Stats[Affix.Def],
                [
                    Stat.SubStats[Affix.Hp],
                    Stat.SubStats[Affix.DefRatio] with { Level = 2 },
                    Stat.SubStats[Affix.AtkRatio] with { Level = 4 },
                    Stat.SubStats[Affix.AnomalyProficiency] with { Level = 2 }
                ]),
            new(DriveDiscSetId.FangedMetal, 4, Rarity.S,
                Stat.Stats[Affix.AnomalyProficiency],
                [
                    Stat.SubStats[Affix.CritRate],
                    Stat.SubStats[Affix.HpRatio],
                    Stat.SubStats[Affix.AtkRatio] with { Level = 3 },
                    Stat.SubStats[Affix.Hp] with { Level = 3 }
                ]),
            new(DriveDiscSetId.FangedMetal, 5, Rarity.S,
                Stat.Stats[Affix.PhysicalDmgBonus],
                [
                    Stat.SubStats[Affix.AnomalyProficiency] with { Level = 2 },
                    Stat.SubStats[Affix.AtkRatio] with { Level = 3 },
                    Stat.SubStats[Affix.Def] with { Level = 2 },
                    Stat.SubStats[Affix.CritDamage] with { Level = 2 }
                ]),
            new(DriveDiscSetId.FangedMetal, 6, Rarity.S,
                Stat.Stats[Affix.AnomalyMasteryRatio],
                [
                    Stat.SubStats[Affix.AnomalyProficiency] with { Level = 2 },
                    Stat.SubStats[Affix.Pen] with { Level = 2 },
                    Stat.SubStats[Affix.DefRatio] with { Level = 3 },
                    Stat.SubStats[Affix.HpRatio] with { Level = 2 }
                ])
        ]);

        CheckStats(reference, jane.BaseStats);
    }

}