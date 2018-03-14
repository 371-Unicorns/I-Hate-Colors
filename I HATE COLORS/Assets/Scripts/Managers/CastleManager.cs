using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the player's castle in the game.
/// </summary>
public class CastleManager : MonoBehaviour
{
    public static int CastleHealth { get; private set; }

    [SerializeField]
    private static GameObject castleWallPrefab;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    public CastleManager() { }

    private void Start()
    {
        CastleHealth = 100;
        castleWallPrefab = (GameObject)Resources.Load("Prefabs/Castle/CastleWall");
        Instantiate(castleWallPrefab, castleWallPrefab.transform.position, castleWallPrefab.transform.rotation, LevelManager.Map.transform);
    }

    public static void AddCastleHealth()
    {
        CastleHealth += 100;
    }

    public static void TakeDamage(int damage)
    {
        CastleHealth = Mathf.Max(CastleHealth - damage, 0);

        if (CastleHealth <= 0)
        {
            GameManager.gameOver = true;
        }
    }
}
