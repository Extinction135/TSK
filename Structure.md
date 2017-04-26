# The Structure of DungeonRun
By structure I'm referring to how the codebase is organized.  
This document will also discuss the paradigms used.  




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
