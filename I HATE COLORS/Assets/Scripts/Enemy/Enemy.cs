using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Collidable {

    private float health;
    private float speed;
    private int value;

    public Enemy() { }

    public void Initialize(float health, float speed, int value) {
        this.health = health;
        this.speed = speed;
        this.value = value;
    }

    public void Initialize(Enemy other)
    {
        this.health = other.health;
        this.speed = other.speed;
        this.value = other.value;
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public override void ProcessCollision(Collidable collidable) 
    {
        if (collidable is Bullet)
        {
            Bullet bullet = (Bullet)collidable;
            
            health -= bullet.GetDamage();
            if (health <= 0)
            {
                GameManager.AddMoney(value);
                EnemyManager.RemoveEnemy(this);
            }

            bullet.ProcessCollision(this);
        }
    }
}
