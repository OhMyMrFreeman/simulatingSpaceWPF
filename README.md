Hello, this code is written to simulate gravity between planets. Also here you can see how I was able to implement a simulation for N-body.

Control buttons:
Left mouse button - adds a moving planet(Green)
Right mouse button - adds a fixed planet(Yellow)
Сenter mouse button - adds a rotating circle of planets(You need to select the type on the left side)

It also uses parallel computing. This heavily loads the processor, but allows you to calculate faster.
To get rid of it, you need to replace Parallel.For in the "PhysicsEngine" file with a regular loop.
