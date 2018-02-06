using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Wave {

    private int id;
    private Queue<Enemy> enemies;

    public Wave() {
        id = -1;
        enemies = new Queue<Enemy>();
    }

    public int EnemiesRemaining()
    {
        return enemies.Count;
    }

    public Enemy NextEnemy()
    {
        return enemies.Dequeue();
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void EnqueueEnemy(Enemy e)
    {
        enemies.Enqueue(e);
    }
}
