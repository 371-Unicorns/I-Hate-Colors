using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
    public static int CastleHealth { get; private set; }

    [SerializeField]
    private static GameObject castleWallPrefab;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private CastleManager() { }

    private void Start()
    {
        CastleHealth = 100;
        castleWallPrefab = (GameObject)Resources.Load("Prefabs/Castle/CastleWall");
        Instantiate(castleWallPrefab, castleWallPrefab.transform.position, castleWallPrefab.transform.rotation);
    }

    public static void TakeDamage(int damage)
    {
        CastleHealth -= damage;
        if (CastleHealth <= 0)
        {
            GameManager.gameOver = true;
        }
    }
}
