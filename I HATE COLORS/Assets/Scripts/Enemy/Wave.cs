using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Wave {

    private int id;
    private Queue<Enemy> enemies;

    public Wave() { }

    public static Wave WaveFromXml(XmlNode waveNode)
    {
        Wave retWave = new Wave();

        retWave.id = int.Parse(waveNode.SelectSingleNode("id").InnerText);
        XmlNodeList enemyNodeList = waveNode.SelectSingleNode("enemies").SelectNodes("enemy");
        foreach (XmlNode enemyNode in enemyNodeList)
        {
            int count = int.Parse(enemyNode.SelectSingleNode("count").InnerText);
            retWave.enemies.Enqueue(EnemyManager.EnemyFromXml(enemyNode.SelectSingleNode("id").InnerText));
        }

        return retWave;
    }

    public int EnemiesRemaining()
    {
        return enemies.Count;
    }

    public Enemy NextEnemy()
    {
        return enemies.Dequeue();
    }
}
