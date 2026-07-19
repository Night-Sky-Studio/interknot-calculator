# Inter-Knot Calculator
Solution for damage calculations at the Inter-Knot project.

<span>
<a href="https://youtrack.interknot.space/issues/IKC" target="_blank"><img src="https://img.shields.io/badge/YouTrack-Issues-purple.svg?logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2NCIgaGVpZ2h0PSI2NCIgZmlsbD0ibm9uZSIgdmlld0JveD0iMCAwIDY0IDY0Ij4KICA8ZGVmcz4KICAgIDxsaW5lYXJHcmFkaWVudCBpZD0iYSIgeDE9Ii0uMTAyNDExIiB4Mj0iNjQuMDUzMiIgeTE9IjMyLjAwMDIiIHkyPSIzMi4wMDAyIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSI+CiAgICAgIDxzdG9wIHN0b3AtY29sb3I9IiNGQjQzRkYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIuOTciIHN0b3AtY29sb3I9IiNGQjQwNkQiLz4KICAgIDwvbGluZWFyR3JhZGllbnQ+CiAgPC9kZWZzPgogIDxwYXRoIGZpbGw9InVybCgjYSkiIGQ9Ik0xLjMwNjM0IDUxLjI0NDZjLS4wOTY4OC0uMDcxMy0uMTI4NzUtLjIwMTktLjA3NjI1LS4zMUw4Ljg2NTcxIDM1LjIwOS4wNTgyMTQgMjQuNjkwOGMtLjA4ODc1LS4xMDU2LS4wNzQzNzUtLjI2MzcuMDMxODc1LS4zNTE4TDI1Ljc3ODggMi45MzIwOUMzMC4zNDk1LS44Nzc5MDggMzYuOTY3LS45ODU0MDggNDEuNjYyNiAyLjY3MTQ3YzQuNjkzNyAzLjY1Njg3IDYuMTkzNyAxMC4wODgxMyAzLjU5NjkgMTUuNDM2MjNsLTIuNzk5NCA1Ljc2NjljMS4wOTE5LS4zNjYzIDIuMTcyNS0uNjk5NCAzLjI0MTItLjk5ODhsMTIuNjczOC0zLjY0MDZjLjEzODEtLjA0LjI4MTkuMDQ1LjMxMzEuMTg1Nmw1LjMwNTYgMjMuNTg1Yy4wMzI1LjE0NTctLjA2NjIuMjg4Mi0uMjE1LjMwNjktMS42ODE4LjIxMTktMTAuODU3NSAxLjUzLTIyLjI4MTIgNi4zMjk0LTEyLjk0MzEgNS40MzYyLTIxLjQ4NDQgMTMuMTYyNS0yMi42OTQ0IDE0LjI5MjUtLjA4OTQuMDgzNy0uMjIwNi4wODY5LS4zMTg3LjAxNDRMMS4zMDYzNCA1MS4yNDQ2WiIvPgogIDxwYXRoIGZpbGw9IiMwMDAiIGQ9Ik01MiAxMkgxMnY0MGg0MFYxMloiLz4KICA8cGF0aCBmaWxsPSIjZmZmIiBkPSJtMjEuNDY2NiAyNi4zNzA5LTUuNDg4MS05LjM3ODdoMy4xNTEzbDMuMzk4MSA1Ljk5MTkuMzk2OS44NTc1LjM5NjgtLjg2ODIgMy4zMTE5LTUuOTgxMmgzLjA5NzVsLTUuNDAyNSA5LjM1NzV2NS42NDg3aC0yLjg2MTl2LTUuNjI3NVoiLz4KICA8cGF0aCBmaWxsPSIjZmZmIiBkPSJNMzMgNDMuOTk4NEgxN3YzaDE2di0zWiIvPgogIDxwYXRoIGZpbGw9IiNmZmYiIGQ9Ik00Mi4zMjQ4IDE2Ljk5MjJIMzAuMjg3OWwtLjAwMDYgMi42MzY5aDQuNTY2MnYxMi4zNjkzaDIuOTI2M1YxOS42MjkxaDQuNTQ1di0yLjYzNjlaIi8+Cjwvc3ZnPgo=" alt="YouTrack - Issues"></a>
<a href="https://youtrack.interknot.space/articles/IKC" target="_blank"><img src="https://img.shields.io/badge/YouTrack-Knowledge_Base-purple.svg?logo=data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSI2NCIgaGVpZ2h0PSI2NCIgZmlsbD0ibm9uZSIgdmlld0JveD0iMCAwIDY0IDY0Ij4KICA8ZGVmcz4KICAgIDxsaW5lYXJHcmFkaWVudCBpZD0iYSIgeDE9Ii0uMTAyNDExIiB4Mj0iNjQuMDUzMiIgeTE9IjMyLjAwMDIiIHkyPSIzMi4wMDAyIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSI+CiAgICAgIDxzdG9wIHN0b3AtY29sb3I9IiNGQjQzRkYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIuOTciIHN0b3AtY29sb3I9IiNGQjQwNkQiLz4KICAgIDwvbGluZWFyR3JhZGllbnQ+CiAgPC9kZWZzPgogIDxwYXRoIGZpbGw9InVybCgjYSkiIGQ9Ik0xLjMwNjM0IDUxLjI0NDZjLS4wOTY4OC0uMDcxMy0uMTI4NzUtLjIwMTktLjA3NjI1LS4zMUw4Ljg2NTcxIDM1LjIwOS4wNTgyMTQgMjQuNjkwOGMtLjA4ODc1LS4xMDU2LS4wNzQzNzUtLjI2MzcuMDMxODc1LS4zNTE4TDI1Ljc3ODggMi45MzIwOUMzMC4zNDk1LS44Nzc5MDggMzYuOTY3LS45ODU0MDggNDEuNjYyNiAyLjY3MTQ3YzQuNjkzNyAzLjY1Njg3IDYuMTkzNyAxMC4wODgxMyAzLjU5NjkgMTUuNDM2MjNsLTIuNzk5NCA1Ljc2NjljMS4wOTE5LS4zNjYzIDIuMTcyNS0uNjk5NCAzLjI0MTItLjk5ODhsMTIuNjczOC0zLjY0MDZjLjEzODEtLjA0LjI4MTkuMDQ1LjMxMzEuMTg1Nmw1LjMwNTYgMjMuNTg1Yy4wMzI1LjE0NTctLjA2NjIuMjg4Mi0uMjE1LjMwNjktMS42ODE4LjIxMTktMTAuODU3NSAxLjUzLTIyLjI4MTIgNi4zMjk0LTEyLjk0MzEgNS40MzYyLTIxLjQ4NDQgMTMuMTYyNS0yMi42OTQ0IDE0LjI5MjUtLjA4OTQuMDgzNy0uMjIwNi4wODY5LS4zMTg3LjAxNDRMMS4zMDYzNCA1MS4yNDQ2WiIvPgogIDxwYXRoIGZpbGw9IiMwMDAiIGQ9Ik01MiAxMkgxMnY0MGg0MFYxMloiLz4KICA8cGF0aCBmaWxsPSIjZmZmIiBkPSJtMjEuNDY2NiAyNi4zNzA5LTUuNDg4MS05LjM3ODdoMy4xNTEzbDMuMzk4MSA1Ljk5MTkuMzk2OS44NTc1LjM5NjgtLjg2ODIgMy4zMTE5LTUuOTgxMmgzLjA5NzVsLTUuNDAyNSA5LjM1NzV2NS42NDg3aC0yLjg2MTl2LTUuNjI3NVoiLz4KICA8cGF0aCBmaWxsPSIjZmZmIiBkPSJNMzMgNDMuOTk4NEgxN3YzaDE2di0zWiIvPgogIDxwYXRoIGZpbGw9IiNmZmYiIGQ9Ik00Mi4zMjQ4IDE2Ljk5MjJIMzAuMjg3OWwtLjAwMDYgMi42MzY5aDQuNTY2MnYxMi4zNjkzaDIuOTI2M1YxOS42MjkxaDQuNTQ1di0yLjYzNjlaIi8+Cjwvc3ZnPgo=" alt="YouTrack - Knowledge Base"></a>
</span>

## Project Structure
Agents, W-Engines (Weapons) and Drive Disc Sets configurations are all represented as Classes.

## How damage is being calculated
*(short ver., long version can be found in the [Damage Calculation](https://youtrack.interknot.space/articles/IKC-A-1) 
article on our Knowledge Base)*

Before everything, the important stuff:
- All characters are assumed to be at level 60 with maxed-out skills (lvl. 12) and Core Skill (lvl. 7).
- All S-rank characters are assumed to be at M0, unless explicitly stated. All A-rank characters are assumed to be at M6.
- All S-rank W-Engines are at P0, A-rank W-Engines are at P6.
- All Support characters have their reference builds that assume maximum supportive bonus and maximum uptime
of any passives.
- Every agent that can provide any value to the original character calculation (Disorder, Abloom, aftershocks, etc.)
has their reference build that uses recommended W-Engine and Drive Disc set with stats set to average values
provided by [Prydwen.gg](https://www.prydwen.gg/zenless) (**not sponsored**).
- **Only your currently equipped Drive Discs are taken into account** for the damage calculation.
- All passives that depend on character swapping, specific attacks combinations or attribute anomalies are
considered to have 100% uptime.
- If you've calculated damage to be higher/lower than the one this program produced - check your calculations and
make sure you've accounted for EVERYTHING. If you did, and you are sure that the calculator is wrong, **open an issue** 
and provide as much information as possible (your UID and character in question with your calculations 
should be enough). Also, make sure that the drive discs you have currently equipped are the same as the ones
you've used in your calculations.
- **We never said anywhere that the calculations should be taken as gospel.** The idea was for them to be replicable 
externally and be as close as possible to the in-game results without going into the rabbit hole of making an entire
game combat simulation engine.

The damage value of each action is calculated independently (previous or future rotation actions don't
have any effect on the damage, with exceptions for agents like Yanagi who can switch stances). 
Calculator uses damage formulas and information described in
[this document](https://youtrack.interknot.space/articles/IKC-A-2).
These formulas can be found in the base `Agent` class.
The Calculator also accounts for all possible passives or buffs/enemy debuffs. When a team is specified, all 
`ExternalBonus` and `ExternalTagBonus` of the teammate will also be accounted for.

## Current Progress

> [!WARNING]
> This list may be incomplete

### Agents
#### Active Agents
- Soldier 11 (1041)
- Miyabi (1091)
- Burnice (1171) + Reference
- Grace (1181)
- Ellen Joe (1191)
- Zhu Yuan (1241)
- Jane Doe (1261) + Reference
- Evelyn (1321)
- Harumasa (1201)
- Yanagi (1221)
- Vivian (1331)
- Soldier 0 - Anby (1381)
- Yixuan (1371)

#### Stun Agents
- [Partial] Lycaon (1141)
- Trigger (1361)
- [Partial] Ju Fufu (1391)

#### Support Agents
- Nicole (1031)
- Soukaku (1131)
- Lucy (1151)
- Rina (1211)
- Astra Yao (1311)
- Pan Yinhu (1421)

### Weapons
- Starlight Engine (13004)
- Precious Fossilized Core (13006)
- Weeping Gemini (13008)
- Electro-Lip Gloss (13009)
- Marcato Desire (13015)
- Cauldron of Clarity (13019)
- The Vault (13103)
- Bashful Demon (13113)
- Kaboom the Cannon (13115)
- Tremor Trigram Vessel (13142)
- Cannon Rotor (14001)
- Hellfire Gears (14110)
- The Brimstone (14104)
- Hailstorm Shrine (14109)
- The Restrained (14114)
- Flamemaker Shaker (14117)
- Fusion Compiler (14118)
- Deep Sea Visitor (14119)
- Zanshin Herb Case (14120)
- Weeping Cradle (14121)
- Timeweaver (14122)
- Riot Suppressor Mark VI (14124)
- Sharpened Stinger (12126)
- Myriad Eclipse (14129)
- Elegant Vanity (12131)
- Heartstring Nocturne (14132)
- Flight of Fancy (14133)
- Spectral Gaze (14136)
- Qingming Birdcage(14137)
- Severed Innocence (14138)
- Roaring Fur-nace (14139)

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
- Dawn's Bloom (33300)
- Moonlight Lullaby (33400)
- White Water Ballad (33500)
- Shining Aria (33600)
- Bunny in Wonderland (33700)
- Notes From the Chained (33800)
- Wuthering Salon (33900)
- The Sky Ablaze (34000)

## Contributing
This project uses .NET NativeAOT compilation, so if you want to introduce any library that uses dynamic code that
conflicts with NativeAOT, your Pull Request might be rejected. Other than that, feel free to contribute and help 
with this project.

## License
```
    Copyright (C) 2025-2026  Night Sky Studio (Konstantin Romanets)

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