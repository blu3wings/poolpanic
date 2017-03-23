# Introduction
This project contain a simple pool game with a simple rule. Two players will take turn to shoot the color ball into the pockets.
The player will get a point each time a colour ball goes into the pocket. If the cue ball goes into the hole, the player will lose a
point. The game ends when there are no more colour balls on the table. Player with the highest point wins the game.

The purpose of this project is to demonstrate that by inheriting a base class, you can create custom behaviour to extend the game.
This is not a complete pool game.

# Important classes and how it's being used

### PoolGameBase.cs
This is the base class for the pool game. It contains methods for tracking balls status, setting up game's basic settings, 
players management and score update.This is the framework where pool game with different rules will be built on.

### SimplePool.cs
The SimplePool class inherits PoolGameBase class, and handles the logic of a simple pool game. It handles UI interaction and the game rule.
One method in particular, VerifyResult is the method for verifiying the rules of the game before passing onto PrepareForNextShot method.
UpdatePoint method will be invoked whenever a colour or cue ball goes into a pocket.

### BallBase.cs
This is the base class for pool balls. It handles all pool balls collision handling, notifies PoolGameBase of its status and award
points to player when it goes into a pocket. A pool ball behaviour can be extended by inheriting this base class.

### PlayerBase.cs
The PlayerBase class stores detail about a player, such as name and score in this project, and notifies UI object whenever a player
switch is made to this object.

### UIBase.cs
The UIBase class contains commonly used methods by the UI elements. In this example, Show(),Hide(), and DisplayToggle().

# Custom classes in this project

### CueStick.cs
  


# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://www.visualstudio.com/en-us/docs/git/create-a-readme). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)
