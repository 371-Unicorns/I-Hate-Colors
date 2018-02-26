using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // TODO doc
    [SerializeField]
    private CoinFly coinFly;

    private float health;
    private float speed;
    private int value;
    private bool dead;

    private Enemy() { }

    public void Initialize(float health, float speed, int value)
    {
        SetHealth(health);
        SetSpeed(speed);
        this.value = value;
        this.dead = false;
    }

    public void Initialize(Enemy other)
    {
        SetHealth(other.health);
        SetSpeed(other.speed);
        this.value = other.value;
        this.dead = other.dead;
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
        if (health <= 0 && !dead)
        {
            dead = true;

            Vector3 enemyScreenPos = Camera.main.WorldToViewportPoint(transform.position);
            foreach (var i in Enumerable.Range(0, value))
            {
                CoinFly newCoinFly = Instantiate(coinFly, GameManager.Instance.canvas.transform);
                RectTransform coinFlyRect = newCoinFly.GetComponent<RectTransform>();
                coinFlyRect.anchorMin = enemyScreenPos;
                coinFlyRect.anchorMax = enemyScreenPos;
                coinFlyRect.anchoredPosition = new Vector2(0, 0);
            }
            GameManager.AddMoney(value);
            this.SetSpeed(0f);
            EnemyManager.RemoveEnemy(this);
        }
    }
}
