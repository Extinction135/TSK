# DungeonRun
A top-down 2D zelda clone, using Monogame 3.6, in a UWP context.  
Tested on both Windows10 and Xbox One.  


## Version 0.1  
![](https://github.com/MrGrak/DungeonRun/blob/master/Gifs/DungeonRun0p1.gif)


## Architecture
The architecture of DungeonRun is best described as functional OOP.  
Great care has been taken to seperate data from functionality.  
However, functionality is grouped into logical global classes.  
Data is passed to these classes methods, from anywhere in the codebase.  
I found this approach reduced the number of instances in the program.  
Subjectively, I think it also makes it easier to reason about program state.  
A great deal of effort has been made to keep the codebase small, clear, and fast.


## Design Patterns
The key guidelines directing development are:    
+ Use the pattern that best solves the problem.  
+ Mix and Overlap patterns when useful.  
+ Traditional wisdom should be challenged, tested, and improved upon.  
+ Don't use more than you need.  


For example, the idea of components (from ECS) are implemented in the codebase.  
An actor is composed of sprite, movement, collision, and input components.  
A gameobject is composed of sprite, movement, and collision components.  
These components are then passed to global classes with functions.  
Sometimes actors or gameobjects are passed to these global classes functions too.  


MVC is also strictly enforced, via the HandleInput(), Update(), and Draw() methods.  
These methods exist in every screen, which are managed by a global ScreenManager class.  
This is based on MSDN examples, but heavily modified and definitely improved upon.  


## More
To see how I'm managing game design, look in the Gameplay and OverviewNotes files.  
To see how I plan and organize my daily commits, look in the ToDo file.  
A goal of mine is to clearly document the commits I make, so check those out too.  



