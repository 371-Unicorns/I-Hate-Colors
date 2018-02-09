using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : Singleton<CastleManager>
{
    public int CastleHealth { get; private set; }

    [SerializeField]
    private GameObject castle;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private CastleManager() { }

    private void Start()
    {
        CastleHealth = 100;
        GameObject castleClone = Instantiate(castle, castle.transform.position, castle.transform.rotation);
        castleClone.SetActive(true);
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
