# JediTrainer 3DUI Proj1 ReadMe

### What I contributed
- Developed the graphical user interface with raytracing for user interaction.
- Created the structure for developing input interactions with OpenXR.
- Designed part of the desert environment. Handled finding the assets for the desert and also founnd TIE Fighter and Millenium Falcon assets.
- Created force elements Heal, Shoot Lightning, and See the Future.
- Created pseudo-random movement for attack droid, and adjusted positional movement of the training droid.
- Created boss movement and enemy spawning functionality for the boss.
- Developed the collision matrix for enemy and player interaction with the world.

### What my partner contributed
- Integrated the OpenXR toolkit for developing with Quest 2.
- Created force elements Push enemies, summon force from all jedi, and force grab / push.
- Integrated the lightsaber and its interaction with the enemies.
- Designed part of the scene including flyover of ships and smoke effect on crashed ship. As well as rocks for throwing at enemies.
- Created health script for the boss and enemies, and found prefabs for both training and attack droid.
- Added sound effects for immersion.

### What Works
Overall the application works very well, all of our force elements are working with added effects to create a more immersive feel to the user. Users can grab a lightsaber and use it to deflect the training droids lasers that they fire at the user, once they finish this round they can destroy attack droids with the lightsaber on collision. In both rounds the user can utilize force powers to aid in attacking the enemies such as throwing in scene objects at them, shooting lightning at them, pushing them backwards, seeing into the future to know the enemy position, and healing themselves when they are hurt. We have successfully implemented a force management system to prevent these abilities from becoming overpowering and forcing the user to use their lightsaber like a Jedi would, the user gains force when they destroy an enemy and loses it any time they use a force power. The next round is a boss fight, in which the user must build up their force meter to full in order to summon the force from all Jedi, this gives them two lightsabers and enough power to destroy the boss enemy, using this move will deplete the user of all of their force and they can only regain it back by destroying enemies that the boss spawns in periodically.

### What Doesn't Work
The quit button on the home page doesn't actually close the user out of the app when built out, it only works in the scene manager in unity. Some audio queues don't function properly, such as when the lightsabers interact with each other and the TIE fighters flying overhead. Sometimes the terrain collider can ignore the lightsabers and cause them to drop straight through the floor.

### Bugs
- In see the future, movement is force based in order to capture where the drone is going which can cause the enemies to overstep where they originally were supposed to move to, this causes a bit of jerky movement.
- When the game ends, it does not technically "end", the UI just prevents the user from effectively interacting with the world.
- Enabling post processing renderer causes game to crash -> Dropped this.
- The boss enemy can spawn enemies inside of itself, which make it hard for the user to know where they are shooting from or are going to attack them from.

### Problems Encountered
- Trying to capture movement for see the future was challenging to smoothly map onto pseudo random movement since I didn't actually know where the enemies would be. I had to create a workaround from capturing positions since it wasn't mapping smoothly and was hard to apply to rotational movement that we had implemented with the training droid, this led to me changing enemy movement from positions to force vectors. I had a lot of challenges with this such as forces stacking accidentally in loops, collisions between the future items and other elements of the scene.
- Originally I had post-processing implemented to create glow effects on some of the users, but I did not have enough research into applying post processing effects to a VR environment, after building and capturing video though I realized I did not have post processing enabled on the main camera and it was causing bugs for that reason.
- Merge conflicts when working with git sometimes caused issues in the scene environment, this project taught me a lot about how to properly resolve merge conflicts with Unity scenes.
- Terrain colliders would sometimes not register with the enemies, causing them to clip through the world, this led me down a rabbit hole of terrain colision and interactions with rigidbodies.
