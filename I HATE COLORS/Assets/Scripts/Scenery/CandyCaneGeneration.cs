using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCaneGeneration : MonoBehaviour {

    public GameObject candyCane;
    public int numCandyCanes;

    private int curCandyCanes;
    private int sceneWidth;
    private int sceneHeight;

	void Start () {
        //sceneWidth = GameManager.Width;
        //sceneHeight = GameManager.Height;
        sceneHeight = 10;
        sceneWidth = 10;
        curCandyCanes = 0;

        //while (curCandyCanes <= numCandyCanes)
        //{
            int posX = Random.Range(0, sceneWidth);
            int posY = Random.Range(0, sceneHeight);
            //GameObject candy = Instantiate(candyCane, new Vector3(1, 2, 0), Quaternion.identity);
        print("sdsdf");
            curCandyCanes++;
       // }
    }
	/*
	void Update () {
        if (curCandyCanes <= numCandyCanes) {
            int posX = Random.Range(0, sceneWidth);
            int posY = Random.Range(0, sceneHeight);
            Instantiate(candyCane, new Vector3(posX, posY, 0), Quaternion.identity);
            curCandyCanes++;
        }
	} */
}
