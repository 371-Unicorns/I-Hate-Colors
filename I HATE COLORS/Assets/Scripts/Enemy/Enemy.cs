using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

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

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.AddMoney(value);
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

    public void printSelf()
    {
        print("health: " + health + ", speed: " + speed + ", value: " + value);
    }
}
