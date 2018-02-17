using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : Singleton<CastleManager>
{
    public int CastleHealth { get; private set; }

    [SerializeField]
    private GameObject castleWallPrefab;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private CastleManager() { }

    private void Start()
    {
        CastleHealth = 100;
        Instantiate(castleWallPrefab, castleWallPrefab.transform.position, castleWallPrefab.transform.rotation);
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
