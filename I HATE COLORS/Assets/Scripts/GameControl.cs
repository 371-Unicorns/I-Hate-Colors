using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public static GameControl instance;
    public bool gameOver;
    public Text gameOverText;
    public Text healthText;

	// Use this for initialization
	void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        gameOver = false;
        gameOverText.text = "";
        healthText.text = "Health: 100";
	}
	
	// Update is called once per frame
	void Update () {
		if(gameOver)
        {
            healthText.text = "Health: 0";
            gameOverText.text = "GAME OVER";
        }
	}
}
