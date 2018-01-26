using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour {

    private int health;
	// Use this for initialization
	void Start () {
        health = 100;
	}
	
	// Update is called once per frame
	void Update () {
        GameControl.instance.healthText.text = "Health: " + health.ToString();
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(health > 0)
            health--;
        if(health == 0)
        {
            GameControl.instance.gameOver = true;
        }
    }
}
