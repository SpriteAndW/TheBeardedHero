----------------------------------------------------------------
Super Retro Collection
©2022 Gif (Twitter @gif_not_jif)

Versions history :
___Version 1.3.0 (June 19th 2022) TilePalette Update ! Tiles reorganisation in a new tilepalette. Also add Resources/Characters/Animals/ subfolder.
___Version 1.2.2 (May 29th 2022) Minor assets parameters update
___Version 1.2.1 (May 24th 2022) Update the marketing thumbnail of this Unity package
___Version 1.2.0 (May 15th 2022) Autotiles update !
___Version 1.1.0 (May 6th 2022) Added Animated sprites and more monsters !
___Version 1.0.0 (May 5th 2022) Creation



----------------------------------------------------------------
What is the Super Retro Collection ? 
Super Retro is a collection of pixelart assets for classic 2D RPG games (turn-based or A-RPG).
You'll get plenty of environments, characters and dungeons full of monsters !



----------------------------------------------------------------
Package structure
Gif/
___README.txt

___Dependency_Unity_Package/..................Dependencies folder for the Super Retro Collection Unity Package
______Ruccho/.................................Dependencies from the content creator Ruccho
_________FangAutoTile/........................Dependency to create autotiles on TilePalette

___Super_Retro_Collection/....................Root path of the Unity Package : Super Retro Collection, by Gif

______Resources/..............................Ressource folder, contains every assets
_________Animations/..........................Animated sprites (frame size is in each filename)
_________ARPG/................................Action-RPG, contains every attack animation for the Characters (swords, spears, staffs, shields, ...)
_________Backgrounds/.........................Battle background for turn-based battle system
_________Battlers/............................Big battlers for turn-based battle system
_________Characters/..........................Characters and monsters (walk animations). A bunch of characters have been separated into subfolders (for the purpose of the playable map, where you can switch character skins)
____________Monsters/.........................Monsters sheets
____________Characters/.......................Characters sheets
____________Animals/..........................Animals sheets

_________Environments/........................Tiles and autotiles to build maps (11 atlas ready to be added on a TilePalette)
____________TilePalette/......................Prebuild TilePalettes (samples)
_______________Autotiles/.....................Autotiles samples
_______________Tiles/.........................Every tiles availables

______Samples/................................A sample playable scene, where you can move around a bunch of assets. CHeck "Notice" section for more infos.

______Scripts/................................Scripts used in the playable map
_________CharacterAppearance..................C# script to animate and change the skin of the 2D Sprite character it is attached to
_________PlayerMovement.......................Move the player character around with the arrow keys



----------------------------------------------------------------
Features 
 - Various environments + dungeons ! Everything you need to build a 2D RPG game (turn-based or A-RPG) !
 - Tiles count : over 7000 ! (16x16) 
 - Animated characters, monsters, chests, doors, and much more !
 - Autotiles and animated autotiles (roads, water, lava, walls, ...)


----------------------------------------------------------------
Notice
I already prepare some elements for you to discover
 - A bunch of autotiles (animated of single frame) arranged in a TilePalette : Gif/Super_Retro_Collection/Ressources/Environments/TilePalette/Auto_Tile_Palette
 - Every tiles arranged in a TilePalette : Gif/Super_Retro_Collection/Ressources/Environments/TilePalette/Full_Tile_Palette
       If the tile palette if too big for you, you can rebuild small ones with the 11 altas available (already splitted here : Ressources/Environments/)
 - I set a small playable scene (Gif/Super_Retro_Collection/Sample/playable_scene) with an animated character which can take difference appearance
       (from "chara_01" to "chara06", stored in Gif/Super_Retro_Collection/Ressources/Characters/)
       You can change its appearance from the public variable "Sprite Sheet Name" in the Component "Character Appearance (Script)" located in the Player/Player_sprite GameObject.
 - ARPG content : each character is provided with many swords, spears, slash and other shield animation (4 directions)



----------------------------------------------------------------
Dependencies (optional)
 - Fang Autotile Unity Package (free) : https://assetstore.unity.com/packages/tools/sprite-management/fang-auto-tile-132602
       The dependency is stored in Gif/Dependencie_Unity_Package



----------------------------------------------------------------
Other demo (available online on itch.io) : https://gif-superretroworld.itch.io/demo-pack



----------------------------------------------------------------
Contact
Twitter @gif_not_jif


Enjoy !