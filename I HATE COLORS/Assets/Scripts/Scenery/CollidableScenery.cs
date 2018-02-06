using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableScenery : Collidable {

    public override void ProcessCollision(Collidable collidable)
    {
        if (collidable is Bullet)
        {
            Bullet bullet = (Bullet)collidable;
            bullet.ProcessCollision(this);
        }
    }
}
