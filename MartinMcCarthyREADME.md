# JediTrainer 3DUI Proj1 ReadMe

### What I contributed
- Developed the graphical user interface with raytracing for user interaction.
- Created the structure for developing input interactions with OpenXR.
- Designed part of the desert environment. Handled finding the assets for the desert and also founnd TIE Fighter and Millenium Falcon assets.
- Created force elements Heal, Shoot Lightning, and See the Future.
- Created pseudo-random movement for attack driud, and adjusted positional movement of the training droid.

### Bugs
- In see the future, movement is force based in order to capture where the drone is going which can cause the enemies to overstep where they originally were supposed to
  move to, this causes a bit of jerky movement.
- When the game ends, it does not technically "end", the UI just prevents the user from effectively interacting with the world.
- Enabling post processing renderer causes game to crash -> Dropped this.

### Problems Encountered
- Trying to capture movement for see the future was challenging to smoothly map onto pseudo random movement since I didn't actually know where the enemies would be. I
  had to create a workaround from capturing positions since it wasn't mapping smoothly and was hard to apply to rotational movement that we had implemented with the
  training droid, this led to me changing enemy movement from positions to force vectors. I had a lot of challenges with this such as forces stacking accidentally in
  loops, collisions between the future items and other elements of the scene.
- Originally I had post-processing implemented to create glow effects on some of the users, but I did not have enough research into applying post processing effects to
  a VR environment, after building and capturing video though I realized I did not have post processing enabled on the main camera and it was causing bugs for that reason.
- Merge conflicts when working with git sometimes caused issues in the scene environment, this project taught me a lot about how to properly resolve merge conflicts with
  Unity scenes.
