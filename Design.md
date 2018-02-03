# The Design of DungeonRun
By design I'm referring to the gameloop and the gameplay.  



## Gameloop
+ Load a dungeon level.
+ Fight enemies, loot gold + items.
+ Defeat the dungeon boss, or die in the dungeon.
+ Display Summary Screen, affect player rating.
+ Return to the Overworld Screen.
+ Load a new dungeon, or purchase upgrades/items at a shop.
+ Repeat.


## Gameplay
+ User inputs commands via a gamepad, like Xbox360 or Xbox One controller.
+ There is no mouse and keyboard input.
+ There are several locations to visit from the Overworld Screen.
+ These include dungeons, and shops.
+ A dungeon is a series of rooms, with enemies, leading up to a boss fight.
+ A shop is a series of rooms, or a single room, where the player can spend gold.
+ Gold is used to purchase items or upgrades for the hero.
+ Items refer to weapons, armor, equipment, magic, etc..
+ Upgrades refer to stat upgrades that changes aspects of the hero.
+ The gameloop repeats until the Final Dungeon is unlocked.
+ Once that dungeon is completed, the game is beaten and the credits roll.
+ Afterwards, the game can continue to be played as normal.



# Additional Development Notes



## UWP App Limitations
+ Target ram useage is < 128mb.
+ This target ram was decided based on this document: https://docs.microsoft.com/en-us/windows/uwp/xbox-apps/system-resource-allocation
+ The max ram available to an app running in the bkg is 128mb.
+ So the game will simply exist in that 128mb available.
+ Monogame framework takes up around 50mb.
+ Sound files take up the most ram, so they're mono at 22k.



## Rendering Optimizations
+ The game renders to a 640x360 texture.
+ This texture is then uniformly scaled to the display resolution.
+ On an Xbox One, this resolution is 1920x1080, so x3 scale.
+ Rendering to 640x360 reduces the number and overhead of draw calls.
+ Draw calls are the most expensive single operation in the game.
+ The Draw loop is the most expensive loop in the game.


## Object Pool Optimizations
+ Object pools are used for Actors, RoomObjects, and Entities.
+ An entity is defined as a projectile, a particle, or an item hero can pickup.
+ An Actor, RoomObject, or Entity is never created outside of a constructor method.
+ This makes the ram useage predictable and stable.
+ This also reduces garbage collection.


## Ai Optimizations
+ 1 to 3 Actors are processed by the AiManager per frame.
+ This number is based on how many active Actors are present on screen.
+ This approach distributes the overhead of Ai across frames.


## Dungeon Generation
+ Dungeons are created using the critical path approach.
+ A series of rooms from the spawn room to the hub room is created.
+ A series of rooms from the hub room to the key room is created.
+ A series of rooms from the hub room to the boss room is created.
+ The critical path is then: spawn -> hub -> key -> hub -> boss.
+ Passing through the hub room twice gives the dungeon a non-linear feel.
+ Additional secret rooms are connected at the end.
+ Secret rooms are connected via bombable walls, in a traditional Zelda style.


## Room Design
+ Rooms are designed by hand via a Room Editor.
+ Rooms are saved to a compressed XML format.
+ All room data is loaded when the game initializes.
+ Room data is copied as Dungeons are generated.
+ The Dungeon layout is random, the Rooms are handmade.

















