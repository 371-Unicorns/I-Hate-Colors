using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;

public class CastleController : MonoBehaviour {

    public int health;
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
        GameControl.GetEnemies().Remove(collision.gameObject);
        Destroy(collision.gameObject);

        if(health > 0)
            health--;
        if(health == 0)
        {
            GameControl.instance.gameOver = true;
        }
    }
}
