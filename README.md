# Multi Agent AI Simulation Using Dynamic Flocking and FSM

## Description

The project simulation consists of a maze where agents from two enemy teams interact. Each team has a captain, controlled by the user, and a group of boids that follow him using a flocking algorithm. When agents from opposing teams meet, they engage in combat; if their health drops to a critical level, they attempt to flee to a safe spot to recover, or they die in battle.

The user can command the captain of any team to move to a specific location, and it will use pathfinding (A*) to trace the optimal route. Additionally, the user can dynamically place walls; agents will recalculate their paths in real-time, avoiding the new structures if they interfere with their movement. Moreover, obstacles can be added to the environment, which agents will detect and avoid using the obstacle avoidance algorithm.

## Features

* Dynamic flocking with smooth pathfinding.

* Finite State Machine for agent behavior transitions.

* Real-time simulation of multiple agents interacting with their environment.

* Collision avoidance and group cohesion.

* Risk assessment for escape routes.

## Technologies Used

* Unity

* C#

* FSM (Finite State Machine) for AI behavior

* Boid Flocking Algorithm for group movement

## Unity Version Recommended: 2022.3.5f1

## How to Interact

* Left Click: Command the Captain of Team 1 to move to the selected location.

* Right Click: Command the Captain of Team 2 to move to the selected location.

* "O" Key: Spawn an obstacle at the mouse pointer's position. Press the "O" Key again while pointing at it to remove it.

* "H" Key: Spawn a horizontal wall at the mouse pointer's position. Press the "H" Key again while pointing at it to remove it.

* "V" Key: Spawn a vertical wall at the mouse pointer's position. Press the "V" Key again while pointing at it to remove it.

## Future Improvements

* Visual improvements and UI implementation.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
