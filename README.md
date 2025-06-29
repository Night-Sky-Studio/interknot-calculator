# Inter-Knot Calculator
Temporary solution for damage calculations. Once the real solution in [Night-Sky-Studio/3ZCalculator](https://github.com/Night-Sky-Studio/3ZCalculator) will be
ready, this repository and all its contents will be archived and never updated. I am not competent enough to make
a really accurate and close-to-the-game calculator, but I am competent enough to make one that won't SEGFAULT when fed
incompatible data.

## Project Structure
Calculator is made to automatically load **Weapons** and **Drive Discs** from `Resources` folder.
Agents are hard-coded, because it's easier to calculate relative stats that way.
All configurations are in JSON format and named with IDs of the corresponding objects. These IDs can be found in
[interknot-calculator/IDs.md](IDs.md) file.

## How damage is being calculated
*(short ver., long version can be found [here](https://github.com/Night-Sky-Studio/interknot-calculator/wiki/Damage-Calculation))*

Before everything, the important stuff:
- All characters are assumed to be at level 70 with maxed out skills (lvl. 12) and Core Skill (lvl. 7).
- All S-rank characters are assumed to be at M0. All A-rank characters are assumed to be at M6. (TODO)
- All S-rank W-Engines are at P0, A-rank W-Engines are at P6. (TODO)
- All Support characters have their reference builds that assume maximum supportive bonus and maximum uptime
of any passives.
- **Only your currently equipped Drive Discs are taken into account** for the damage calculation.
- All passives that depend on character swapping, specific attacks combinations or attribute anomalies are
considered to have 100% uptime.
- If you've calculated damage to be higher/lower than the one this program produced - check your calculations and
make sure you've accounted for EVERYTHING. If you did, and you are absolutely sure that the calculator is wrong -
**open an issue** and provide as much information as possible (your UID and character in question with your calculations 
should be enough). Also make sure that the drive discs you have currently equipped are the same as the ones
you've used in your calculations.
- **We never said anywhere that the calculations should be taken as gospel.** The idea was for them to be replicable 
externally and be as close as possible to the in-game results without going into the rabbit hole of making an entire
game combat simulation engine.

The damage of each action is calculated independently (previous or future rotation actions don't
have any effect on the damage). Calculator uses damage formulas and information described in
[this document](https://docs.google.com/document/d/e/2PACX-1vSo82Ac3HqdI_G5_BoAqYJToK6LX4FGLPJxjPZEbhMQ-wSyFyxDFl1dr8i5czcCLJmYwxWfsXkCXN6v/pub).
Those formulas could be found in the `Calculator`'s `GetStandardDamage()` and `GetAnomalyDamage()` functions.
The Calculator also accounts for all possible passives or buffs/enemy debuffs. When a team is specified, all 
`ExternalBonus` and `ExternalTagBonus` of the teammate will also be accounted for.

## Current Progress
### Agents
#### Active Agents
- Soldier 11 (1041)
- Miyabi (1091)
- Burnice (1171)
- Grace (1181)
- Ellen Joe (1191)
- Zhu Yuan (1241)
- Jane Doe (1261)
- Evelyn (1321)
- Harumasa (1201)

#### Stun Agents
- [Partial] Lycaon (1141)

#### Support Agents
- Nicole (1031)
- Soukaku (1131)
- Lucy (1151)
- Rina (1211)
- Astra Yao (1311)

### Weapons
- Electro-Lip Gloss (13009)
- Marcato Desire (13015)
- The Vault (13103)
- Bashful Demon (13113)
- Kaboom the Cannon (13115)
- Cannon Rotor (14001)
- The Brimstone (14104)
- Hailstorm Shrine (14109)
- The Restrained (14114)
- Flamemaker Shaker (14117)
- Fusion Compiler (14118)
- Deep Sea Visitor (14119)
- Zanshin Herb Case (14120)
- Weeping Cradle (14121)
- Riot Suppressor Mark VI (14124)
- Sharpened Stinger (12126)
- Elegant Vanity (12131)
- Heartstring Nocturne (14132)

### Drive Disc Sets
- Woodpecker Electro (31000)
- Puffer Electro (31100)
- Shockstar Disco (31200)
- Freedom Blues (31300)
- Hormone Punk (31400)
- Soul Rock (31500)
- Swing Jazz (31600)
- Chaos Jazz (31800)
- Proto Punk (31900)
- Inferno Metal (32200)
- Chaotic Metal (32300)
- Thunder Metal (32400)
- Polar Metal (32500)
- Fanged Metal (32600)
- Branch & Blade Song (32700)
- Astral Voice (32800)
- Shadow Harmony (32900)
- Phaethon's Melody (33000)
- Yunkui Tales (33100)
- King of the Summit (33200)

## Contributing
This project uses .NET NativeAOT compilation, so if you want to introduce any library that uses dynamic code that
conflicts with NativeAOT, your Pull Request might be rejected. Other than that, feel free to contribute and help 
with this project.

## License
```
    Copyright (C) 2025  Night Sky Studio (Konstantin Romanets)

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
```
For more details, see the [full license text](https://www.gnu.org/licenses/gpl-3.0.txt).