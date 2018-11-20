





# The Design of DungeonRun


(these notes are in development)  


## Game + GamePlay Design

+ the visual, auditory, and temporal game experience
+ how it looks, sounds, and interacts with the player

+ using lttp as reference/source material for sprites
+ using lttp colors migrated to a smaller 21 pallete scheme
+ designing in black and white at first
+ migrating to color, and the changes (boomerang, color frequency)
+ the overworld, it's design, and how it links to levels
+ dungeon entrance and location design, based on overworld visuals
+ improving the boomerang
+ improving dashing
+ improving ground fires
+ improving swimming, diving
+ improving carrying, throwing in various states
+ climbing, falling, jumping, grabbing while falling
+ pets! 
+ coliseum, the economy and the restricted wallet, gambling




## Codebase Design

+ codebase design, paradigms and ideas used
+ why ideas were used and how they fit together 
+ refers to: globals, ecs, functional, object, procedural, declarative
+ screens, screen manager, global classes and data
+ game input, actor input, program input (keyboard, mouse, gamepad)

+ the main game loop
+ collisions vs interactions, and systems seperation
+ interactions, movement, animation, resolving collisions as last step
+ interactions affecting movement, animation, etc...

+ what is a room? what is a level? what is a levelSet?
+ dungeon rooms, and their xml savedata, and roomIds
+ fields, and their xml savedata, and roomIds
+ converting xml to cs files, referencing those files in project
+ why leveldata is compiled to binary, visual studio limitations

+ dungeons, dungeon rooms, previous dungeon generation algorithms
+ the critical path approach, multiple critical paths (key)
+ changing textures, spawning enemies, minibosses, bosses
+ tricks (key behind secret room) and hacks

+ actors, gameobjects, projectiles - and their components
+ component functions, and why they are seperated and used
+ actors, gameobjects, projectiles - and their pools
+ object pools, the global pool, why pools are used

+ strategies for keeping the code as fast as possible
+ keeping data structures as small as possible
+ being aware of how 'large' the data moving thru registers is
+ comparison size of boolean, byte, int, float, double, etc..


## Overworld Design  
  
+ combining super mario brothers on snes' overworld with lttp's flute overworld
+ why the flute doesn't exist in game: it became a dedicated overworld map
+ then the idea of fields vs dungeon rooms was added to the overworld map  
+ link loads a field from overworld map, then explores into dungeons, other fields, etc..
+ skulls are the pervasive visual theme, along with tropical environment locale
+ overworld is largely based on lttp's overworld map, complete with clouds in the corners
+ the dungeon/skull destructionn is a wip right now, but i think it's going to be awesome






#Old Design Notes

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
+ Target ram useage used to be < 128mb.
+ As of 2018, UWP game ram can be up to 5GB.
+ This target ram was decided based on this document: https://docs.microsoft.com/en-us/windows/uwp/xbox-apps/system-resource-allocation
+ I still want to keep the ram as small as possible.
+ So, total ram footprint should be less than 1gb.
+ Monogame framework takes up around 50mb.
+ Sound files take up most ram, so they're mono at 22k.


## Platform Agnostic Design Principles
+ Keep RAM footprint small and frametimes low.
+ Keep Game code separate from platform specific code.
+ These two principles make it much easier to port the game.
+ A lower power system can still likely run the game at 60fps.
+ Only a Functions_Backend class copy needs to be written for the platform.


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

















