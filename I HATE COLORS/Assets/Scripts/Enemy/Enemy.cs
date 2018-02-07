using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Collidable {

    private float health;
    private float speed;
    private int value;

    public Enemy() { }

    public void Initialize(float health, float speed, int value) {
        SetHealth(health);
        SetSpeed(speed);
        this.value = value;
    }

    public void Initialize(Enemy other)
    {
        SetHealth(other.health);
        SetSpeed(other.speed);
        this.value = other.value;
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
        gameObject.GetComponent<Pathfinding.AIPath>().maxSpeed = speed;
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
