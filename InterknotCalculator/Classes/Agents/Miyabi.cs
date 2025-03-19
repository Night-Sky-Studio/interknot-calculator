using InterknotCalculator.Enums;

namespace InterknotCalculator.Classes.Agents;

// public class Miyabi : Agent {
//     public Miyabi() : base(1091) {
//         Speciality = Speciality.Anomaly;
//         Element = Element.Ice;
//         Rarity = Rarity.S;
//
//         Stats[Affix.Hp] = 7673;
//         Stats[Affix.Def] = 606;
//         Stats[Affix.Atk] = 880;
//         Stats[Affix.CritRate] = 0.05;
//         Stats[Affix.CritDamage] = 0.5;
//         Stats[Affix.Impact] = 86;
//         Stats[Affix.AnomalyMastery] = 116;
//         Stats[Affix.AnomalyProficiency] = 238;
//         Stats[Affix.IceDmgBonus] = 0.3;
//
//         Anomalies["Frostburn"] = new(1500, Element.Ice, []);
//
//         Skills["Kazahana"] = new() {
//             Tag = SkillTag.BasicAtk,
//             Scales = [
//                 new (54.4, 21.2, Element.Physical),
//                 new (59.3, 41.2, Element.Physical),
//                 new (126.6, 70.6),
//                 new (193.3, 123.9),
//                 new (258.8, 197.0)
//             ]
//         };
//         Skills["Shimotsuki"] = new() {
//             Tag = SkillTag.BasicAtk,
//             Scales = [
//                 new(910.1, 66.0),
//                 new(1717.2, 97.2),
//                 new(4282.8, 567.0),
//             ],
//             Affixes = {
//                 { Affix.DmgBonus, 0.6 },
//                 { Affix.IceResPen, 0.3 }
//             }
//         };
//         Skills["Fuyubachi"] = new() {
//             Tag = SkillTag.Dash,
//             Scales = [new(52.2, 19.5, Element.Physical)]
//         };
//         Skills["KanSuzume"] = new() {
//             Tag = SkillTag.Counter,
//             Scales = [new(492.3, 322.3)]
//         };
//         Skills["DancingPetals"] = new() {
//             Tag = SkillTag.QuickAssist,
//             Scales = [new(209.0, 157.3)]
//         };
//         Skills["FallingPetals"] = new() {
//             Tag = SkillTag.DefensiveAssist,
//             Scales = [
//                 new (0, 407.7),
//                 new (0, 514.4),
//                 new (0, 193.2)
//             ]
//         };
//         Skills["Miyuki"] = new() {
//             Tag = SkillTag.Special,
//             Scales = [
//                 new (72.1, 54.5)
//             ]
//         };
//         Skills["Hisetsu"] = new() {
//             Tag = SkillTag.ExSpecial,
//             Scales = [
//                 new(788.3, 483.4),
//                 new(967.2, 608.5)
//             ]
//         };
//         Skills["SpringsCall"] = new() {
//             Tag = SkillTag.Chain,
//             Scales = [new(1258.3, 284.7)]
//         };
//         Skills["LingeringSnow"] = new() {
//             Tag = SkillTag.Ultimate,
//             Scales = [new(4776.1, 556.3)]
//         };
//     }
// }