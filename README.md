# DungeonRun - v0.77 - 2018.11.07
+ My secret 2D Zelda game.. Keep this between us, ok?   
  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/077release.gif)  

+ 0.77 playthrough: https://youtu.be/_b7qclGTffE  
+ You can play my game right now, for free, if you are on Windows 10.  
+ You can also play my game on Xbox 1 right now, for free. See below.  


## Playing the Game - Windows 10
+ You can install the DirectX version or the UWP version on Windows 10:
+ 0.77 DX: https://www.dropbox.com/s/sppi2wilhef1qso/TSK_077_Dx.zip?dl=0  
+ 0.77 UWP: https://www.dropbox.com/s/9r0xcyfhilc99mo/TSK_077_UWP.zip?dl=0  


## Playing the Game - Xbox1
+ You can sideload the UWP version onto your Xbox one using this guide:  
+ https://gbatemp.net/threads/how-to-sideload-dungeonrun-indie-game-on-xbox-one.470374/  


## Media
+ youtube channel with weekly video content:  
+ https://www.youtube.com/channel/UCsgpfZB1USgAe0Lv2ZI18uw  
+ to see a bunch of cool gifs and learn about the timeline, look at the VERSIONS page.


## Goals
+ Modern 2D Zelda, based on 'golden age' of nintendo game development (90s).
+ Incorporates Miyamoto's missing ideas from Lttp (grass fires, ditches) <<< done.
+ Extends Miyamoto's ideas (swimming underwater, climbing, etc..) <<< done.
+ Procedurally generated dungeons based on a multiple critical path approach <<< done.
+ Handmade rooms, which are stitched together into cohesive dungeons <<< done.
+ Overworld map like SMB with changing locations based on story progress <<< wip.
+ Play around with the visual style of 90s-era gameboy games <<< wip.
+ Dark, mature storyline where link becomes an anti-hero and real characters die <<< wip.
+ Multi-stage boss and miniboss battles, colliseum tournaments <<< done.


## Specs
+ 60 fps @ 16:9 ratio  
+ 640x360 base resolution scaled up to 720/1080/etc 
+ Xbox 360/One controller support
+ Runs on Windows 10 & XboxOne (via UWP sideloading)
+ Designed to be easily ported to PS4, Linux, Switch, etc..
+ Doesn't require much processing power  


## Contributing
+ It's likely Nintendo will want/try to stop this. 
+ But I think there is potential in completing this.
+ So please help me keep this project quiet until v1.0.  
+ Starring, cloning, and forking are fine. Just dont tell nintendo, reddit or 4chan. Please. 
+ https://gamingreinvented.com/nintendoarticles/top-ten-nintendo-fanworks-cancelled-due-legal-complaints/  


## Older Versions
+ version 0.76 is the last prototype version, in black and white.  
+ 0.76 DirectX: https://www.dropbox.com/s/zwsua9p3051m4lc/DR_076_Dx.zip?dl=0  
+ 0.76 UWP: https://www.dropbox.com/s/nasl9aecdfyzyvf/DR_076_UWP.zip?dl=0  


## Input
+ The game was designed to be played with an Xbox360 or XboxOne controller.  
+ You can play with a similar supported controller, or the Keyboard.  
+ Keyboard Input is WASD based for movement and JILK based for buttons.  
+ Keyboard input is not mappable right now, but will be in a future update.  


## Dependencies
+ You'll need .Net4.5, and maybe SharpDx and Monogame installed.
+ The DirectX Install archive has been tested to work on Windows10.
+ The UWP Install archive has been tested to sideload on Xbox1.
+ If these install archives don't work for you, then Clone, Open and Build.
+ Clone the Repo to your local machine, then OPEN the VS project for Dx or UWP.
+ You'll need Monogame and a compatible IDE installed to BUILD it (F5).
+ All the information you need is provided in the Getting Started Guide.
+ https://github.com/MrGrak/Monogame-Getting-Started


## Intro To The Codebase  
+ Start with DataClasses, at the top.  
+ This is where you'll find the control flags to put the game into release mode.  
+ You can also put the game into edit mode, and play around with the level or room editor.  
+ Enumerators and DataClasses contain most of the data.
+ Game Logic is contained within Function classes in /GameClasses.
+ Game State is managed via Screens and ScreenManager.  
+ I suggest using VisualStudio and the shortcut Ctrl+M, +O to navigate the regions.  
+ Good Luck.  









