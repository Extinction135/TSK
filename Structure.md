# The Structure of DungeonRun
By structure I'm referring to how the codebase is organized.  
This document will also discuss the paradigms used.  




## Architecture
The architecture is best described as a cross between OOP and Functional programming.  
Great care has been taken to seperate data from functionality.  
However, functionality is grouped into classes.  
Data is passed to these classes methods, from anywhere in the codebase.  
Methods attempt to be pure, but this is not always true.  
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
A component, actor, or gameobject is then passed into a method, along with other parameters.  
No interfaces, factories, or additional abstractions are used.  


The MVC pattern is also used, via the HandleInput(), Update(), and Draw() methods.  
These methods exist in every screen, which are managed by a ScreenManager class.  
This is based on MSDN examples, but modified to suit my needs.  
