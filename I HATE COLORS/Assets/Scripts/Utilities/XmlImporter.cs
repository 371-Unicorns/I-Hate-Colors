using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XmlImporter
{

    private static readonly string ENEMIES_XML_FILEPATH = "Resources/XML/enemies.xml";
    private static readonly string WAVE_COMPOSITION_FILEPATH = "Resources/XML/wave_composition.xml";

    private static Dictionary<string, Enemy> enemies = null;
    private static List<Wave> waves = null;
    private static Dictionary<string, Tower> towers = null;

    public static List<Wave> GetWavesFromXml()
    {
        if (waves != null)
        {
            return waves;
        }

        if (enemies == null)
        {
            enemies = GetEnemiesFromXml();
        }

        List<Wave> retList = new List<Wave>();

        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + "/" + WAVE_COMPOSITION_FILEPATH);

        XmlNode root = doc.SelectSingleNode("waves");
        foreach (XmlNode node in root.SelectNodes("wave"))
        {
            Wave wave = new Wave();

            wave.SetId(int.Parse(node.SelectSingleNode("id").InnerText));
            XmlNodeList enemyNodeList = node.SelectSingleNode("enemies").SelectNodes("enemy");
            foreach (XmlNode enemyNode in enemyNodeList)
            {
                int count = int.Parse(enemyNode.SelectSingleNode("count").InnerText);
                for (int i = 0; i < count; i++)
                {
                    Enemy e = enemies[enemyNode.SelectSingleNode("id").InnerText];

                    wave.EnqueueEnemy(e);
                }
            }

            XmlNode n = node.SelectSingleNode("spawn-rate");
            if (n != null)
            {
                wave.SetSpawnRate(float.Parse(n.InnerText));
            }

            retList.Add(wave);
        }

        if (waves == null)
        {
            waves = retList;
        }

        return retList;
    }

    public static Dictionary<string, Enemy> GetEnemiesFromXml()
    {
        if (enemies != null)
        {
            return enemies;
        }

        Dictionary<string, Enemy> retDictionary = new Dictionary<string, Enemy>();
        Transform prefabHolder = GameObject.Find("PrefabHolder").transform;

        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + "/" + ENEMIES_XML_FILEPATH);

        foreach (XmlNode node in doc.SelectSingleNode("enemies").SelectNodes("enemy"))
        {
            string id = node.SelectSingleNode("id").InnerText;
            float health = float.Parse(node.SelectSingleNode("health").InnerText);
            float speed = float.Parse(node.SelectSingleNode("speed").InnerText);
            int value = int.Parse(node.SelectSingleNode("value").InnerText);

            Enemy e = (GameObject.Instantiate(Resources.Load("Prefabs/Enemies/" + id)) as GameObject).GetComponent<Enemy>();
            e.transform.Translate(new Vector3(-1000, -1000, 0));
            e.Initialize(health, speed, value);
            e.transform.SetParent(prefabHolder);

            retDictionary.Add(id, e);
        }

        if (enemies == null)
        {
            enemies = retDictionary;
        }

        return retDictionary;
    }

    public static Dictionary<string, Tower> GetTowersFromXml()
    {
        if (towers != null)
        {
            return towers;
        }

        Dictionary<string, Tower> retDictionary = new Dictionary<string, Tower>();

        XmlDocument doc = new XmlDocument();
        doc.Load(Application.dataPath + "/" + ENEMIES_XML_FILEPATH);

        Transform prefabHolder = GameObject.Find("PrefabHolder").transform;

        foreach (XmlNode node in doc.SelectSingleNode("towers").SelectNodes("tower"))
        {
            string id = node.SelectSingleNode("id").InnerText;
            float damage = float.Parse(node.SelectSingleNode("damage").InnerText);
            float fireRate = float.Parse(node.SelectSingleNode("fire-rate").InnerText);
            float range = float.Parse(node.SelectSingleNode("fire-rate").InnerText);
            int cost = int.Parse(node.SelectSingleNode("cost").InnerText);

            Tower tower = (GameObject.Instantiate(Resources.Load("Prefabs/Towers/" + id)) as GameObject).GetComponent<Tower>();
            tower.transform.Translate(new Vector3(-1000, -1000, 0));
            tower.Initialize(id, damage, range, fireRate, cost);
            tower.transform.SetParent(prefabHolder);

            retDictionary.Add(id, tower);
        }

        if (towers == null)
        {
            towers = retDictionary;
        }

        return retDictionary;
    }
}
