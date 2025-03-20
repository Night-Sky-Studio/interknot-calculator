# Inter-Knot Calculator
Temporary solution for damage calculations. Once the real solution in [Night-Sky-Studio/3ZCalculator](https://github.com/Night-Sky-Studio/3ZCalculator) will be
ready, this repository and all its contents will be archived and never updated. I am not competent enough to make
a really accurate and close-to-the-game calculator, but I am competent enough to make one that won't SEGFAULT when fed
incompatible data and one that won't send back 500 errors if it can't process something.

## Project Structure
Calculator is made to automatically load **Agents**, **Weapons** and **Drive Discs** from `Resources` folder.
All configurations are in JSON format and named with IDs of the corresponding objects. These IDs can be found in
[interknot-calculator/IDs.md](IDs.md) file.

## Current Progress
### Agents
- Miyabi (1091)
- Jane Doe (1261)

### Weapons
- Hailstorm Shrine (14109)
- Sharpened Stinger (14126)

### Drive Disc Sets
- Woodpecker Electro (31000)
- Freedom Blues (31300)
- Polar Metal (32500)
- Fanged Metal (32600)
- Branch & Blade Song (32700)
- Astral Voice (32800)


## Contributing
This project uses .NET NativeAOT compilation, so if you want to introduce any library that uses dynamic code that
conflicts with NativeAOT, your Pull Request might be rejected. Other than that, feel free to contribute and help 
with this project.

## License
```
    Copyright (C) 2025  Night Sky Studio (Konstantin Romanets)

    This program is free software: you can redistribute it and/or modifyΩ
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