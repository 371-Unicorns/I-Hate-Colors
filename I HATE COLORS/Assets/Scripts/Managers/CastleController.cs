using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : Singleton<CastleController>
{
    public int CastleHealth { get; private set; }

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private CastleController() { }

    private void Start()
    {
        CastleHealth = 100;
    }

    public void TakeDamage(int damage)
    {
        CastleHealth -= damage;
        if (CastleHealth <= 0)
        {
            GameManager.Instance.gameOver = true;
        }
    }
}
