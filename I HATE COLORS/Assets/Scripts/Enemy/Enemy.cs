using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Generic enemy.
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private BloodFly bloodFly;

    private float health;
    private float speed;
    private int value;
    private bool dead;
    private ColorType color;

    private Enemy() { }

    /// <summary>
    /// Initializes enemy w/ starting values
    /// </summary>
    /// <param name="health">float</param>
    /// <param name="speed">float</param>
    /// <param name="value">int</param>
    /// <param name="color">ColorType</param>
    public void Initialize(float health, float speed, int value, ColorType color)
    {
        SetHealth(health);
        SetSpeed(speed);
        this.value = value;
        this.dead = false;
        this.color = color;
    }

    /// <summary>
    /// Initializing new enemy with given enemy's values
    /// </summary>
    /// <param name="other">Enemy</param>
    public void Initialize(Enemy other)
    {
        SetHealth(other.health);
        SetSpeed(other.speed);
        this.value = other.value;
        this.dead = other.dead;
        this.color = other.color;
    }

    /// <summary>
    /// Sets enemy's health
    /// </summary>
    /// <param name="health">float</param>
    private void SetHealth(float health)
    {
        this.health = health;
    }

    /// <summary>
    /// Sets the enemy's speed
    /// </summary>
    /// <param name="speed">float</param>
    private void SetSpeed(float speed)
    {
        this.speed = speed;
        gameObject.GetComponent<Pathfinding.AIPath>().maxSpeed = speed;
    }

    /// <summary>
    /// Returns whether the enemy is dead
    /// </summary>
    /// <returns>Dead boolean</returns>
    public bool isDead() { return dead; }

    /// <summary>
    /// Take damage and check whether for death.
    /// </summary>
    /// Authors: Courtney Chu, Steven Johnson, Cole Twitchell
    /// <param name="damage">Damage to take.</param>
    public void TakeDamage(float damage, ColorType damageType)
    {
        if (damageType == ColorType.BLACK || damageType == color)
        {
            health -= damage * 2.0f;
        }
        else
        {
            health -= damage;
        }

        if (health <= 0 && !dead)
        {
            dead = true;

            // Setup bloodFly
            Vector3 enemyScreenPos = Camera.main.WorldToViewportPoint(transform.position);
            BloodFly newBloodFly = Instantiate(bloodFly, GameManager.canvas.transform);
            RectTransform bloodFlyRect = newBloodFly.GetComponent<RectTransform>();
            bloodFlyRect.anchorMin = enemyScreenPos;
            bloodFlyRect.anchorMax = enemyScreenPos;
            bloodFlyRect.anchoredPosition = new Vector2(0, 0);

            GameManager.AddMoney(value);
            this.SetSpeed(0f);
            EnemyManager.RemoveEnemy(this);
        }
    }
}
