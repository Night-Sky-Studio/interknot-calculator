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

## Current Progress
### Agents
- Soldier 11 (1041)
- Miyabi (1091)
- Ellen Joe (1191)
- Zhu Yuan (1241)
- Jane Doe (1261)

### Weapons
- Hailstorm Shrine (14109)
- Riot Suppressor Mark VI (14124)
- Sharpened Stinger (14126)
- Electro-Lip Gloss (13009)
- Fusion Compiler (14118)
- The Brimstone (14104)
- Canon Rotor (14001)
- Deep Sea Visitor (14119)
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