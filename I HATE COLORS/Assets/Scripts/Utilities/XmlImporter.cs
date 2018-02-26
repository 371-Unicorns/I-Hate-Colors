using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XmlImporter
{

    private static readonly string ENEMIES_XML_FILEPATH = "XML/enemies";
    private static readonly string WAVE_COMPOSITION_FILEPATH = "XML/wave_composition";
    private static readonly string TOWERS_XML_FILEPATH = "XML/towers";

    private static Dictionary<string, Enemy> enemies = null;
    private static Queue<Wave> waves = null;
    private static Dictionary<string, Tower> towers = null;

    public static Queue<Wave> GetWavesFromXml()
    {
        if (waves != null)
        {
            return waves;
        }

        if (enemies == null)
        {
            enemies = GetEnemiesFromXml();
        }

        Queue<Wave> retQueue = new Queue<Wave>();

        XmlDocument doc = new XmlDocument();
        TextAsset textasset = (TextAsset)Resources.Load(WAVE_COMPOSITION_FILEPATH, typeof(TextAsset));
        doc.LoadXml(textasset.text);

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

            retQueue.Enqueue(wave);
        }

        if (waves == null)
        {
            waves = retQueue;
        }

        return retQueue;
    }

    public static Dictionary<string, Enemy> GetEnemiesFromXml()
    {
        if (enemies != null)
        {
            return enemies;
        }

        Dictionary<string, Enemy> retDictionary = new Dictionary<string, Enemy>();

        XmlDocument doc = new XmlDocument();
        TextAsset textasset = (TextAsset)Resources.Load(ENEMIES_XML_FILEPATH, typeof(TextAsset));
        doc.LoadXml(textasset.text);

        foreach (XmlNode node in doc.SelectSingleNode("enemies").SelectNodes("enemy"))
        {
            string id = node.SelectSingleNode("id").InnerText;
            float health = float.Parse(node.SelectSingleNode("health").InnerText);
            float speed = float.Parse(node.SelectSingleNode("speed").InnerText);
            int value = int.Parse(node.SelectSingleNode("value").InnerText);

            Enemy e = GameObject.Instantiate(Resources.Load("Prefabs/Enemies/" + id) as GameObject, LevelManager.Instance.PrefabHolderParent).GetComponent<Enemy>();
            e.Initialize(health, speed, value);

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
        TextAsset textasset = (TextAsset)Resources.Load(TOWERS_XML_FILEPATH, typeof(TextAsset));
        doc.LoadXml(textasset.text);

        Transform prefabHolder = GameObject.Find("PrefabHolder").transform;

        foreach (XmlNode node in doc.SelectSingleNode("towers").SelectNodes("proj-tower"))
        {
            string id = node.SelectSingleNode("id").InnerText;
            float projectileDamage = float.Parse(node.SelectSingleNode("proj-damage").InnerText);
            float projectileSpeed = float.Parse(node.SelectSingleNode("proj-speed").InnerText);
            float fireRate = float.Parse(node.SelectSingleNode("fire-rate").InnerText);
            float range = float.Parse(node.SelectSingleNode("range").InnerText);
            int cost = int.Parse(node.SelectSingleNode("cost").InnerText);
            int upgradeCost = int.Parse(node.SelectSingleNode("upgrade-cost").InnerText);
            double upgradeCostScale = double.Parse(node.SelectSingleNode("upgrade-cost-scale").InnerText);
            int maxLevel = int.Parse(node.SelectSingleNode("max-level").InnerText);

            ProjectileTower tower = (GameObject.Instantiate(Resources.Load("Prefabs/Towers/TowerPrefabs/" + id)) as GameObject).GetComponent<ProjectileTower>();
            tower.transform.Translate(new Vector3(-1000, -1000, 0));
            tower.Initialize(id, cost, upgradeCost, upgradeCostScale, maxLevel, range, fireRate, projectileSpeed, projectileDamage);
            tower.transform.SetParent(prefabHolder);

            retDictionary.Add(id, tower);
        }

        foreach (XmlNode node in doc.SelectSingleNode("towers").SelectNodes("aoe-tower"))
        {
            // initialize aoe towers here
        }

        foreach (XmlNode node in doc.SelectSingleNode("towers").SelectNodes("dot-tower"))
        {
            string id = node.SelectSingleNode("id").InnerText;
            float effectDamage = float.Parse(node.SelectSingleNode("dot-damage").InnerText);
            float range = float.Parse(node.SelectSingleNode("range").InnerText);
            int cost = int.Parse(node.SelectSingleNode("cost").InnerText);
            int upgradeCost = int.Parse(node.SelectSingleNode("upgrade-cost").InnerText);
            double upgradeCostScale = double.Parse(node.SelectSingleNode("upgrade-cost-scale").InnerText);
            int maxLevel = int.Parse(node.SelectSingleNode("max-level").InnerText);

            DoTTower tower = (GameObject.Instantiate(Resources.Load("Prefabs/Towers/TowerPrefabs/" + id)) as GameObject).GetComponent<DoTTower>();
            tower.transform.Translate(new Vector3(-1000, -1000, 0));
            tower.Initialize(id, cost, upgradeCost, upgradeCostScale, maxLevel, range, effectDamage);
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
