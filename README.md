# 1PP-controller (in development)

A simple first-person controller with `[W][S][A][D]` controls, `[LShift]` to acceleration, mouse camera control and ability to jump via `[Space]`

:warning: Required **Capsule object** with **Rigidbody** and **Camera object** as a child. Camera must be specified in `Character Camera field` in Inspector.

Jump ability works only with the ground objects that have Layer ID equal 6
(need to create new layer in inspector with ID: 6 and name it Ground)

![image](https://github.com/ViaKotov/1PP-controller/assets/89484940/0386f3cd-db99-452a-91dd-4249176831f4)

![image](https://github.com/ViaKotov/1PP-controller/assets/89484940/8fa4f767-835e-459f-952d-c0d208af7469)

or you could just change the scripts value LayerMask.NameToLayer  **"Ground"** to something else

![image](https://github.com/ViaKotov/1PP-controller/assets/89484940/497b4aaa-f041-40ef-b509-478d6c14c68b)



:green_circle: Things i'm planning to do:
- [x] Camera soft motion
- [ ] Crouch ability
- [ ] Climbing ability
- [ ] Jump off the wall
- [ ] Shoot system
- [ ] Pickup and interact system
- [ ] Sound system
