using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health;
    private float speed;
    private int value;

    private Enemy() { }

    public void Initialize(float health, float speed, int value)
    {
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

    private void SetHealth(float health)
    {
        this.health = health;
    }

    private void SetSpeed(float speed)
    {
        this.speed = speed;
        gameObject.GetComponent<Pathfinding.AIPath>().maxSpeed = speed;
    }

    /// <summary>
    /// Take damage and check whether for death.
    /// </summary>
    /// <param name="damage">Damage to take.</param>
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.AddMoney(value);
            this.SetSpeed(0f);
            EnemyManager.RemoveEnemy(this);
        }
    }
}
