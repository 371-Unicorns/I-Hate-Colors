using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pathfinding;

namespace MainGame
{
    public class GameControl : MonoBehaviour
    {

        public static GameControl instance;
        public GameObject castle;
        public bool gameOver;
        public Text gameOverText;
        public Text healthText;

        public GameObject[] enemyList;

        private static ArrayList activeEnemies = new ArrayList();

        // Use this for initialization
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            gameOver = false;
            gameOverText.text = "";
            healthText.text = "Health: 100";
        }

        // Update is called once per frame
        void Update()
        {
            if (gameOver)
            {
                healthText.text = "Health: 0";
                gameOverText.text = "GAME OVER";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                SpawnEnemy(0);
            }
        }

        private void SpawnEnemy(int idx)
        {
            GameObject obj = Instantiate(enemyList[0]);
            obj.GetComponent<AIDestinationSetter>().SetTarget(castle);
            activeEnemies.Add(obj);
        }

        public static ArrayList GetEnemies()
        {
            return activeEnemies;
        }
    }
}