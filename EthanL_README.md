# JediTrainer 3D UI Assignment 1

## Contributions
#### Force Interaction #1,2,3,6
* Coded:
  * force grab controller script. Spent a lot of time trying to get force grabbing/movement/throwing to feel intuitive.
  * Lightsaber interaction. Expanding, closing, and interaction with the environment, and ambient SFX.
  * Connected UI to the player so it updated properly.
* Designed the final game environment including dynamic props like ships in the background to make the environment feel more alive. As well as particle effects for the crashed ship, enemy death, and force healing.
* Created and implemented SFX for ambient noise, force powers, and enemy death.
* Refined custom inputs to detect left and right-handed inputs separately.

<br>

This project was very interesting but also very limited on time. I feel like what I have now is just a demo but it could be further refined to make it a full-fledged game that I would be proud to put on my portfolio/resume. With more time I would have added the ability to force grab groups of objects, added more SFX, especially for lightsaber collision and swings, and finalized the game loop. I think development would have been better paced over 2 projects. The first project should be focused on dialing in the force interactions and the second should focus on environment, effects, and sounds. This way students can spend more time refining the XR inputs which I found to be the most valuable learning experience from this first project.

<br>

### Partner's Contributions
#### Force Interaction #1,4,5
* Made the menu screen
* Made health/force UI
* Created half the XR inputs
* Designed the Terrain and static props
* Made particle effect for lightning
* Coded Enemy Movement/Collision

## Known Bugs
* Force-throwing objects at enemies instantly destroys them but should only take off 1 health. 
* The lightning particle effect loops infinitely on one hand when input is held.
* The lightsaber falls through the floor with no consistent reason.
* The blade of the lightsaber will sometimes get slightly offset when using force on it.
* I had to remove the lightsaber collision noises cause it would collide with the handle and make constant noise.
