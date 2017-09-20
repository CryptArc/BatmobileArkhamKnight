# Batman Arkham Knight - Batmobile

## Vehicle setup
* Rigidbody with mass 1500
* Box collider around the vehicle model's mesh
* 4 Wheel colliders with default values
* Adjustable steer angle and torque through code

## Drive and Combat modes
|Drive mode|Combat mode|
|----------|-----------|
|![drivemode](https://media.giphy.com/media/l1J9xZRJUsIQo27Je/giphy.gif)|![combatmode](https://media.giphy.com/media/xT9IgEEF6C890tOYiQ/giphy.gif)|

* The Batmobile is in Drive mode by default and vehicle physics are applied to it (WASD keys)
* It can be switched to Combat mode by pressing and holding down Left Shift key
* While in Combat mode, vehicle physics are not applied, rather it is moved using Rigidbody.MovePosition()

## Camera settings
- A camera's position is offset by height and depth from the Batmobile and follows it
- Camera's rotation and position in Drive mode
  - Rotation is set to the vehicle's Y axis rotation
  - Position is set using these equations
  ```C
    X = Batmobile.position.X + cos(Batmobile's Y axis rotation in degrees) * depthOffset
    Y = Height offset
    Z = Batmobile.position.Z + sin(Batmobile's Y axis rotation in degrees) * depthOffset
   ```
- Camera's rotation and position in Combat mode
  - Rotation is set using Mouse X movement
  - Position is set in a similar way as of Drive mode, only replacing Batmobile's Y axis angle with Camera's Y axis

## Adjusting the steer angle with respect to velocity
Y = (0.6)^(X/8) * 35

Where Y is Steer angle, X is Velocity

![Steer angel vs Velocity](https://image.ibb.co/c07jtk/desmos_graph.png)

