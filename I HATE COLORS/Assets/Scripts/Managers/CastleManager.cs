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

    /// <summary>
    /// Gives Castle a starting health point and instantiates the object from the prefab
    /// </summary>
    /// Edited by Courtney Chu
    private void Start()
    {
        CastleHealth = 100;
        castleWallPrefab = (GameObject)Resources.Load("Prefabs/Castle/CastleWall");
        Instantiate(castleWallPrefab, castleWallPrefab.transform.position, castleWallPrefab.transform.rotation, LevelManager.Map.transform);
    }

    /// <summary>
    /// Adds 100 health to the castle
    /// </summary>
    public static void AddCastleHealth()
    {
        CastleHealth += 100;
    }

    /// <summary>
    /// Castle takes given amount of damage
    /// </summary>
    /// Edited by Courtney Chu
    /// <param name="damage">int</param>
    public static void TakeDamage(int damage)
    {
        CastleHealth = Mathf.Max(CastleHealth - damage, 0);

        if (CastleHealth <= 0)
        {
            GameManager.gameOver = true;
        }
    }
}
