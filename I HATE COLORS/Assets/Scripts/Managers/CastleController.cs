using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class CastleController : Singleton<CastleController>
{

    public int health;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private CastleController() { }

    // Use this for initialization
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.castleHealth = health;
        GameManager.Instance.healthText.text = "Health: " + health.ToString();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //GameControl.GetEnemies().Remove(collision.gameObject);
        //Destroy(collision.gameObject);

        if (health > 0)
            health--;
        if (health == 0)
        {
            GameManager.Instance.gameOver = true;
        }
    }
}
