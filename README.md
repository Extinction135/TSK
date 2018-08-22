# DungeonRun - v0.76 - 2018.8.22
+ A combination of several Zelda games, circa 1995-1997.  
  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/076presentation.gif)   
  
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
+ Starring, cloning, and forking are fine. Just dont tell reddit or 4chan. Please. 
+ https://gamingreinvented.com/nintendoarticles/top-ten-nintendo-fanworks-cancelled-due-legal-complaints/  


## Playing the Game (Windows 10 + Xbox1)
+ 0.75 DirectX: https://www.dropbox.com/s/mqsawbx4ceexxbf/DR_075_Dx.zip?dl=0  
+ 0.75 UWP: https://www.dropbox.com/s/3rak0eosi8m0q5h/DR_075_UWP.zip?dl=0  
+ 0.76 DirectX: https://www.dropbox.com/s/zwsua9p3051m4lc/DR_076_Dx.zip?dl=0  
+ 0.76 UWP: https://www.dropbox.com/s/nasl9aecdfyzyvf/DR_076_UWP.zip?dl=0  
+ You can sideload the UWP version onto your Xbox one using this guide:  
+ https://gbatemp.net/threads/how-to-sideload-dungeonrun-indie-game-on-xbox-one.470374/  


## Dependencies
+ You'll need a compatible gamepad (Xbox360 or XboxOne controller) to play.
+ You'll also need .Net4.5, and possibly SharpDx and Monogame installed.
+ The game has been tested to run on Windows 10 and Xbox1.
+ Alternatively, you can clone the repo and build the game yourself.
+ You'll need Monogame and a compatible IDE installed to build it.
+ All the information you need is provided in the Getting Started Guide.
+ https://github.com/MrGrak/Monogame-Getting-Started

## Intro To The Codebase  
+ Start with DataClasses, at the top.  
+ This is where you'll find the control flags to put the game into release mode.  
+ You can also put the game into edit mode, and play around with the level or room editor.  
+ Enumerators and DataClasses contain most of the data.
+ Game Logic is contained within MANY Function_Whatever classes in /GameClasses.
+ Game State is managed via Screens and ScreenManager.  
+ I suggest using VisualStudio and the shortcut Ctrl+M, +O to navigate the regions.  
+ I could write several books about the codebase, so I'll stop right here.  
+ Good Luck.  









