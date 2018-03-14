using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Procedural ganeration of scenery on the grid.
/// </summary>
public class SceneryGenerator : MonoBehaviour
{
    /// <summary>
    /// How many candy cane trees are procedurally generated.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    private static readonly int NUM_CANDYCANES = 9;

    /// <summary>
    /// The candy cane tree drawn by Amy.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public GameObject candyCane;

    /// <summary>
    /// Procedurally generates candy cane trees in the game.
    /// These trees can provide protection for enemies blocking projectiles from hitting them if they are behind one.
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public static void GenerateScenery()
    {
        int curCandy = 0;

        while (curCandy < NUM_CANDYCANES)
        {
            int posX = Random.Range(-GameManager.Width / 2, GameManager.Width / 2);
            int posY = Random.Range(-GameManager.Height / 2, GameManager.Height / 2);
            GameObject candy = GameObject.Instantiate(Resources.Load("Prefabs/Scenery/candycane"),
                                                      new Vector3(posX, posY, 0),
                                                      Quaternion.identity) as GameObject;
            candy.transform.SetParent(GameObject.Find("Scenery").transform);
            curCandy++;
        }
    }
}
