using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryGenerator : MonoBehaviour
{

    private static readonly int NUM_CANDYCANES = 9;

    public GameObject candyCane;
    public int numCandyCanes;

    public static void GenerateScenery()
    {
        int curCandy = 0;

        while (curCandy < NUM_CANDYCANES)
        {
            int posX = Random.Range(-GameManager.Instance.Width / 2, GameManager.Instance.Width / 2);
            int posY = Random.Range(-GameManager.Instance.Height / 2, GameManager.Instance.Height / 2);
            GameObject candy = GameObject.Instantiate(Resources.Load("candycane"),
                                                      new Vector3(posX, posY, 0),
                                                      Quaternion.identity) as GameObject;
            candy.transform.SetParent(GameObject.Find("Scenery").transform);
            curCandy++;
        }
    }
}
