# FPS-controller (in development)

A simple first-person controller

## Controls
-  `[W][S][A][D]` - Movement
-  `[LShift]` - Acceleration
-  `[Space]` - Jump
-  `[LCtrl]` - Crouch

## Roadmap
:green_circle: Things i'm planning to do (things that already done):
- [x] :white_check_mark: Camera soft motion
- [x] :white_check_mark: Crouch ability
- [x] :white_check_mark: Climbing ladders ability
- [ ] :hourglass: Basic UI elements
- [ ] :hourglass: Shoot system
- [ ] :hourglass: Pickup and interact system
- [ ] :hourglass: Sound system


## How to use
:warning: Required **Capsule object** with **Rigidbody** and **Camera object** as a child. Camera must be specified in `Character Camera field` in Inspector.

### Jumping
Jump ability works only with the ground objects that have Layer ID equal 6
(need to create new layer in inspector as **User Layer 6** and name it **Ground**)

![image](https://github.com/ViaKotov/1PP-controller/assets/89484940/0386f3cd-db99-452a-91dd-4249176831f4)

![image](https://github.com/ViaKotov/1PP-controller/assets/89484940/8fa4f767-835e-459f-952d-c0d208af7469)

or you could just change the script string value LayerMask.NameToLayer  **"Ground"** to something else

![image](https://github.com/ViaKotov/1PP-controller/assets/89484940/497b4aaa-f041-40ef-b509-478d6c14c68b)

### Ladder Climbing
My way to realize ladder climbing was to create primitive object, change it collider position and scale it.
![image](https://github.com/ViaKotov/FPS-controller/assets/89484940/9de6190a-8483-40db-bba0-292b989a3201)

I'd checked `Is Trigger` in object's collider component in Inspector.

![image](https://github.com/ViaKotov/FPS-controller/assets/89484940/36e3e1fd-46b1-4884-ad92-6ce7d8644898)

And there is separate script called `interLadder`, that you can edit. 

:warning: The only condition to make it work fine is to assing character object tag **Player**

