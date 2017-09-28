# Batman Arkham Knight - Batmobile

## Vehicle setup
* Rigidbody with mass 500
* Box collider around the vehicle model's mesh
* Adjustable steer angle and torque through code

## Drive Mode

|Afterburner|Drive to Combat mode shift|
|-----------|--------------------------|
|![Afterburner](https://media.giphy.com/media/l378bihKBrkVyUo7K/giphy.gif)|![D2C](https://media.giphy.com/media/xT9Igk2TDghEMqpvlm/giphy.gif)|

## Combat Mode

|Weapon pop|Combat dodge|
|----------|------------|
|![Weaponpop](https://media.giphy.com/media/3o7aDa4eJCwHKrC6GY/giphy.gif)|![Dodge](https://media.giphy.com/media/3o7aDdAcUnvr0lNb7q/giphy.gif)|

* The Batmobile is in Drive mode by default and vehicle physics are applied to it
* It can be switched to Combat mode by pressing and holding down Left trigger
* While in Combat mode, vehicle physics are not applied, rather it is moved using Rigidbody.MovePosition()

## Camera settings
- A camera's position is offset by height and depth from the Batmobile and follows it
- Camera shakes while using boost
- Camera's rotation and position in Drive mode
  - Rotation is set to the vehicle's Y axis rotation
  - Position is set using these equations
  ```C
    X = Batmobile.position.X + cos(Batmobile's Y axis rotation in degrees) * depthOffset
    Y = Height offset
    Z = Batmobile.position.Z + sin(Batmobile's Y axis rotation in degrees) * depthOffset
   ```
- Camera's rotation and position in Combat mode
  - Rotation is set using Right stick's input
  - Position is set in a similar way as of Drive mode, only replacing Batmobile's Y axis angle with Camera's Y axis
