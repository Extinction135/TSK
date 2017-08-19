# Versions
Each version of DungeonRun expands on the game's mechanics and design.  


## Initial Repo Creation - 2017.03.16 (+0 days, +3 commits)  
+ Pushed prototype.
+ Setting aside time to work on project.


## Development Begins - 2017.04.01 (+16 days, +0 commits)  
+ Development part-time begins.
+ Part-time is about 4-5 hours a day.


## Version 0.1 - 2017.04.15 (+15 days, +130 commits)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p1.gif)  
+ Player controls a Hero character via a Gamepad.
+ Hero can move, dash, and attack blob enemies.
+ Blob enemies can move, dash, and attack hero.
+ A summary screen displays various data about the player's performance.
+ A basic single-room dungeon contains the hero and blobs.
+ Extensive debugging information is provided about program & object state.


## Version 0.2 - 2017.04.25 (+10 days, +135 commits)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p2A.gif)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p2B.gif)
+ A Boss enemy has been added, with it's own sounds and Ai.
+ A Boss room has been added, with transitions between rooms.
+ Chests and rewards have been added.
+ The hero must have the dungeon key to open the Boss Door.
+ This key can only be obtained from a chest.
+ An inventory menu has been added, but not yet implemented.
+ Various visual improvements, including a Boss Door floor decal.


## Version 0.3 - 2017.05.05 (+10 days, +131 commits)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p3A.gif)  
+ Hero can cast fireball projectiles.
+ Added Overworld map screen, with selectable locations.
+ Hero can now exit and enter dungeons.
+ Added new dungeon objects: moving spikeblock, conveyor belt, and bumper.
+ Additional chest rewards: gold, key, map, and heart pieces.
+ Heart pieces can be collected to increase total hearts.
+ Improved boss battle with additional mobs spawned.
+ Improved Inventory screen, with animations.
+ Gameloop completed: overworld map -> dungeon -> summary screen -> overworld map.
+ Dungeon music plays for dungeon screen, randomly selected between two music tracks.
+ Title music plays for Summary and Overworld Screens.


## Version 0.4 - 2017.05.20 (+15 days, +153 commits)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p4A.gif)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p4B.gif)  
+ Added the ability to purchase various items from vendors.
+ Added a shop level, where vendors sell their products.
+ Added a dialog widget so NPCs can communicate with player.
+ Added a for sale widget, so player can purchase vendor items.
+ Implemented arrows, bombs, health and magic potions.
+ Implemented the fairy in a bottle item.
+ Implemented pickups for magic, arrows, and bombs.
+ Redesigned the world UI to display a magic meter.
+ Updated fireball to cost 1 magic to cast, but deals 2 damage now.
+ Implemented 4 different armor types: cloth, plate, cape, and robes.
+ Implemented the ring equipment item, which increases the loot drop rate.
+ Created products for bombs and arrows, to be purchased from item vendor.
+ Created the bow product, which allows hero to fire arrows.
+ Created the flamethrower room object, which shoots fireballs.
+ Created the wall statue room object, which shoots arrows.


## Version 0.5 - 2017.07.03 (+44 days, +112 commits)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p5A.gif)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p5B.gif)   
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p5C.gif)  
+ Added ability to save and load game files.
+ Added ability to create new game files, and handle corrupt game files.
+ Added title screen, with logo and background image.
+ Added load/save/new screen, which handles game files.
+ Changed how dungeons were created, in prep for RoomBuilder tool.
+ Reorganized project to allow for sharing of core game classes.
+ This shared setup allows porting of game to DirectX & OpenGL much easier.
+ Created skeleton DirectX project for RoomBuilder tool.
+ Created closing animation for menuRectangles.
+ Widgets and Screens now have opening and closing animations.
+ Created crystals widget. Changed Design of stats widget.
+ Redesigned inventory screen with different widget layout.
+ Various bug fixes, refactoring to separate data and functionality.
+ Various refactoring of classes, reduction of code, simplifications.
+ Lots of UX work on screens, and transitions between them.


## Version 0.6 - 2017.08.19 (+47 days, +164 commits)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p6A.gif)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p6B.gif)   
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p6C.gif)  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p6D.gif)  
+ Added RoomBuilder (editor), ability to save/load rooms as XML.
+ Updated Overworld with new map and animated sprites.
+ Created Dungeon Generation routine.
+ Complete, randomly generated dungeons are now created.
+ Added dungeon map to show room layout + connections.
+ Implemented various cheats in-game via Flags class.
+ Tons of visual & audio improvemtents, bug fixes.





