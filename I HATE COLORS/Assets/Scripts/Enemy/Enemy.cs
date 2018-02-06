using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private float health;
    private float speed;

    public Enemy() { }

    public Enemy(float health, float speed) // number one
    {
        this.health = health;
        this.speed = speed;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemyManager.RemoveEnemy(this);
        }
    }

    public void SetHealth(float health)
    {
        this.health = health;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public int GetValue()
    {
        return 20;
    }
}
