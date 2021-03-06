﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents one wave to send to the player. Most fields are to be initialized during wave creation using the XMLImporter.
/// 
/// Author: Cole Twitchell
/// </summary>
public class Wave {

    private float spawnRate = 1.0f;

    private int id;
    private Queue<Enemy> enemies;

    private GameTimer unitSpawnTimer;

    public Wave() {
        id = -1;
        enemies = new Queue<Enemy>();

        unitSpawnTimer = new GameTimer();
        unitSpawnTimer.SetTimer(spawnRate);
    }

    public void Update()
    {
        unitSpawnTimer.Update();
    }

    public int EnemiesRemaining()
    {
        return enemies.Count;
    }

    public Enemy NextEnemy()
    {
        unitSpawnTimer.Reset();

        return enemies.Dequeue();
    }

    public int GetId() { return id; }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetSpawnRate(float rate)
    {
        this.spawnRate = rate;
        unitSpawnTimer.SetTimer(rate);
    }

    /// <summary>
    /// Adds an enemy to the wave.
    /// </summary>
    /// <param name="e"></param>
    public void EnqueueEnemy(Enemy e)
    {
        enemies.Enqueue(e);
    }

    /// <summary>
    /// Returns true if the wave is ready to spawn a new enemy, false otherwise.
    /// </summary>
    /// <returns></returns>
    public bool SpawnReady()
    {
        return !unitSpawnTimer.IsPaused() && unitSpawnTimer.IsDone() && EnemiesRemaining() > 0;
    }
}
