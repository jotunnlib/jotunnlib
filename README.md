# JotunnLib
Jötunn (_/ˈjɔːtʊn/, "giant"_) Lib is a modding library for Valheim, with the goal of making the lives of mod developers easier.  
Get the mod on [NexusMods](https://www.nexusmods.com/valheim/mods/507) or [Thunderstore](https://valheim.thunderstore.io/package/JotunnLibTeam/JotunnLib/)!

## ❗❗ Deprecation notice ❗❗
**JotunnLib is no longer under active development**, and has not been for quite some time. If you are looking to create a mod, **please use [Jötunn, the Valheim Library](https://github.com/Valheim-Modding/Jotunn) instead!** If you are installing JotunnLib as a dependency for any mods, go ahead, but please bug the mod author to make the switch to JVL instead :)

The only updates that will be made to JotunnLib going forward will be to fix any compatibility issues or game-breaking issues - no new features will be added.

[Jötunn, the Valheim Library](https://github.com/Valheim-Modding/Jotunn) is the direct successor to JotunnLib. It was originally created by the teams behind ValheimLib and JotunnLib as a means to unify the Valheim modding community, and it has evolved into a massive project with many contributors from the community. 

Thank you again to all the developers and players who tried out and used the original JotunnLib ❤️

## Installation
Instructions for installing and using JotunnLib as a user:

1. You will first need to download [BepInEx for Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).  
    Move the contents of BepInExPack_Valheim folder into `<Steam Location>\steamapps\common\Valheim`.  
    _Note: If you already have used any other mods, you likely already have this installed._
2. Download JotunnLib from the [Releases](https://github.com/jotunnlib/jotunnlib/releases) page. 
    Unzip the downloaded files into `<Steam Location>\steamapps\common\Valheim\BepInEx\plugins`.
3. That's it! Now download some mods which use JotunnLib! :)

## Features & Roadmap
Currently, JotunnLib lets you create & add all of the following custom things into Valheim:

- [x] Custom prefabs
- [x] Custom inventory items
- [x] Custom recipes
    - [ ] Create using JSON
- [x] Custom input buttons
    - [ ] Ability to change custom keybinds in-game via settings menu
- [x] Custom skills
    - [ ] Add using JSON
- [x] Custom localizations for current language
    - [ ] Localizations for other languages
    - [ ] Localizations from JSON file
- [x] Custom piece tables (create your own variant of the Hammer, Cultivator, etc.)
    - [ ] Create using JSON
- [x] Custom pieces to existing piece tables (adding extra items to Hammer, Cultivator, etc.)
    - [ ] Add using JSON
- [x] Custom vegetation spawning in the world
- [x] Utils for loading custom assets at runtime
    - [x] Loading 2D textures
    - [x] Loading meshes from .obj model files
- [ ] Custom commands
    - [x] Custom console commands
    - [ ] Custom chat commands
- [ ] Listening to game events
    - [x] (Currently very few event listeners implemented)
    - [ ] All game events
- [ ] Custom tabs for in-game settings menu

## Developing mods
Visit our [documentation site](https://jotunnlib.github.io/jotunnlib) for more info.

## Repo structure
The repository is split up into a few parts:
- Code relating to JotunnLib is in the [JotunnLib](https://github.com/jotunnlib/jotunnlib/tree/main/JotunnLib) folder
- Documentation source for JotunnLib is within the [JotunnLib/Documentation](https://github.com/jotunnlib/jotunnlib/tree/main/JotunnLib/Documentation) folder
- Demo mod used as an example is in the [TestMod](https://github.com/jotunnlib/jotunnlib/tree/main/TestMod) folder

## Issues
Have any issues or feature requests? Open a [pull request](https://github.com/jotunnlib/jotunnlib/pulls) or submit an [issue](https://github.com/jotunnlib/jotunnlib/issues)!

## Contributing
For information about contributing to the repo, see the [Contributing instructions](CONTRIBUTING.md).
