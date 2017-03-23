# Introduction
This project contain a simple pool game with a simple rule. Two players will take turn to shoot the color ball into the pockets.
The player will get a point each time a colour ball goes into the pocket. If the cue ball goes into the hole, the player will lose a
point. The game ends when there are no more colour balls on the table. Player with the highest point wins the game.

The purpose of this project is to demonstrate that by inheriting a base class, you can create custom behaviour to extend the game.
This is not a complete pool game.

# Getting Started
1. Setup
2. How to play
3. Settings and Parameters
4. Important classes
5. Custom classes
6. Known issues

# Setup
1. Clone [this reposity](https://github.com/blu3wings/poolpanic)
2. Open SimplePool game in Unity3D Ver 5.5
3. Go to File -> Build Settings, switch to Android platform (stand alone PC and Mac will put the UI alignment out of position)
4. Run the game.

# How to play
1. Click on the "Let's get it started" button to begin.
2. To move the aim guide line, use the left mouse button click, hold and move the mouse. Release the mouse button to stop. If the top and button UI start to disappear, not a problem, it is intended that way to make more room for you to move your mouse cursor around.
3. To shoot, click and move the slider button at the bottom of the screen to set the shot power, release to shoot. To cancel the shot, move the slider all the way to the left.
4. At the end of the game, click on the "Play again" button to restart the game.

# Settings and Parameters (Name in hierarchy)
1. Velocity Decay Rate - 0 to 1. This changes how much speed the ball will lose as it travel across the table. The closer it's to 0, the faster it will stop. (SimplePool)
2. Bounce Threshold - This dictates how much velocity is required for an object to bounce off each other. The higher the number, the faster an object needs to travel. It is set to 0.1 as default. (SimplePool)
3. Shot Force Multiplier - This value multiplies the value from the shot slider. The higher the value, more force is applied to the cue ball. (SimplePool)
4. Cue stick sensitivity - The higher the value, the more sensitive the cue stick will get. (CueStick)
5. Points - Points to award to a player each time the ball goes into the pocket. (ColourBall #)

# Important classes

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
This class handles the aim guide line and the cue stick rotation. Each time the rotation of the cue stick is changed, SimplePool class will be notified of the direction.

### ShotSliderPanel.cs
The ShotSliderPanel applies the force to the PoolBall object by moving the slider. 

### GameResetPanel.cs
The GameResetPanel displays game result message when the game ends. Clicking on the button in the panel will reset the game to its orginal state and the game can be played again.

### GameStartPanel.cs
Self explanatory, this class handles input to start the game.

### PlayerPanel.cs
This class handles the player's score and notify the player's turn.

# Known Issues
1. Cue stick rotates differently depending on where your mouse cursor position is when you initiate the move.
2. At the start of the game, if you try to shoot immediately, the cue will not move. This is caused by missing direction vector.

Created by Danny Chow // 2017 Oni Factory
