using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class CastleController : MonoBehaviour
{
    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private CastleController() { }

    private void Start()
    {
        GameManager.Instance.castleHealth = 100;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //GameControl.GetEnemies().Remove(collision.gameObject);
        //Destroy(collision.gameObject);

        if (GameManager.Instance.castleHealth > 0)
            GameManager.Instance.castleHealth--;
        if (GameManager.Instance.castleHealth == 0)
        {
            GameManager.Instance.gameOver = true;
        }
    }
}
