Multiplayer Pacman - Assignment 3

Daniel Wiktorczyk - 40060894 

The code is also available at https://github.com/danielwiktorczyk/Multiplayer-Pacman , which will be made public after the final submission due date


In this write up, I will cover each section of the assignment3 individually.


Notably, **the extra special features** I have added include: 

- *Each* ghost has their own personality--also similar to classic pacman. 
	Blinky chases directly after the closest pacman
	Pinky will try to ambush pacman by aiming for the spot several tiles ahead of the closest pacman
	Inky and Clyde, for simplicity, try to ambush the closest pacman from his left and right side similar to Pinky
	I used the **strategy design pattern** to accomplish this
	Please use the **pause and play** scene view to see the Debug.lines involved. 
		Note that these decisions are made only when a ghost is near / at the center of a tile, so you'll need a few next frames to see the lines again
- I use a **buffered input system**, similar to classic pacman:
	If pacman cannot yet make a turn in the inputted direction, it is added to the buffered
	The buffer stores the latest input direction that cannot yet be done (e.g. a wall blocks)
	Once pacman enters a corridor that allows the buffered input, the buffered input is applied
	This allows for a wider *window of oppertunity* for input
- I restricted ghost movement such that they may only ever go forwards; 
	**in the spirit of pacman, ghosts do not double back** (unless they change their phase, which is out of scope of this assignment hehe), 
	which provides more strategy oppertunities for the player
- There's a countdown UI that begins only when both players succesfully join. 

To play, there's only one scene, which is added to the build 

If you want to test just the gameplay features atomically: 
- *disable Player2*. 


R1 	Level Environment
*All parts completed as follows:*
- The level is 28 x 31 tiles, with every element fitting within a 1x1x1 cube
- All elements are 3D, viewed isometrically such that shadows do not get in the way
- The level is based on the traditional stage, but without the ghost-cage requirement, it reforms the center so that it may be better used (ghosts start at the top instead)
- Exit tunnels located on the left and right sides
- Pellets are edible and hover over the floor (check Scene view, since the camera view does not show this)
- There are no sharp turns


R2	Networked Multiplayer Pacman Game:
*All parts completed as follows:*

2.1 
- Pacman is shaped as a vertical hockey puck (despite being depicted as a sphere in most google search images results??)
- Pellets are eaten when a collision between a player and a pellet is detected. 
- A sound effect plays when eaten 
- Collision between PCs are ignored
- Game ends when all pellets are eaten, with a UI displaying who won
- Pellet eating sounds are only heard by the client player, as instructed

2.2 
- In the traditional 4 corners of the stage, larger and flashing "speed" pellets are placed, which provides a temporary ~5s speed boost

2.3 
- The PC movement is restricted to orthogonal direction, and WASD / UpLeftDownRight are available. 
- No PC nor Ghost deviates from the path, such that wall collision is avoided. Hopefully ;) 
- The PCs start very close to each other in the middle of the map (but not overlapping), which I deemed fair due to symmetry

2.4 
- As mentioned in the *extra special features*, I implemented 4 ghosts with unique personalities / attacking strategies! Again: 
- *Each* ghost has their own personality--also similar to classic pacman. 
	Blinky chases directly after the closest pacman
	Pinky will try to ambush pacman by aiming for the spot several tiles ahead of the closest pacman
	Inky and Clyde, for simplicity, try to ambush the closest pacman from his left and right side similar to Pinky
	I used the **strategy design pattern** to accomplish this
	Please use the **pause and play** scene view to see the Debug.lines involved. 
		Note that these decisions are made only when a ghost is near / at the center of a tile, so you'll need a few next frames to see the lines again
- Again, please note the ghost movement is restricted so that it may never double back, which is intended, as explained in my *extra special features* above
- I tried a different approach to path finding, without requiring a complex algorithm. 
	Instead, ghosts determine which direction will will have them get closest to a pacman 
	This, coupled with the personality ambushing, allows for a computational cheap yet surprisingly effective AI for ghosts working as a team--at least I find! 
- On collision with a PC, that PC is Spooked! and goes back to its starting position. A sound also plays here too! 
	
2.5 
- WIP the other pacman's and ghosts' movements are not updated as quickly. 

I tried to maintain a consistent coding style, with regular extraction of methods and naming of variable. Also, I have divided the file structure frequently. 


I hope you enjoy, and Thank you! 

