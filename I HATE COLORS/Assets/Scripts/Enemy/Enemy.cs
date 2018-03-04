using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void Initialize(float health, float speed, int value, ColorType color)
    {
        SetHealth(health);
        SetSpeed(speed);
        this.value = value;
        this.dead = false;
        this.color = color;
    }

    public void Initialize(Enemy other)
    {
        SetHealth(other.health);
        SetSpeed(other.speed);
        this.value = other.value;
        this.dead = other.dead;
        this.color = other.color;
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

    public bool isDead() { return dead; }

    /// <summary>
    /// Take damage and check whether for death.
    /// </summary>
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
            TowerInformation.CheckUpgrade();
            this.SetSpeed(0f);
            EnemyManager.RemoveEnemy(this);
        }
    }
}
