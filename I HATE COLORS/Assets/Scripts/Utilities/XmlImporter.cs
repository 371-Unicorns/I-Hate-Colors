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
    private static Dictionary<string, Tower> towers = null;
    private static Queue<Wave> waves = null;

    /// <summary>
    /// Returns Queue waves full of enemies from XML file
    /// 
    /// Edited by Courtney Chu
    /// </summary>
    /// <returns>Queue of waves</returns>
    public static Queue<Wave> GetWavesFromXml()
    {
        if (waves != null)
        {
            return waves;
        }

        Dictionary<string, Enemy> enemies = GetEnemiesFromXml();

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

        waves = retQueue;

        return retQueue;
    }

    /// <summary>
    /// Returns Dictionary of enemies from XML file
    /// 
    /// Edited by Courtney Chu
    /// </summary>
    /// <returns>Dictionary of enemies</returns>
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
            ColorType color = (ColorType)ColorType.Parse(typeof(ColorType), node.SelectSingleNode("color").InnerText);

            Enemy e = (GameObject.Instantiate(Resources.Load("Prefabs/Enemies/" + id), LevelManager.PrefabHolderParent) as GameObject).GetComponent<Enemy>();
            e.Initialize(health, speed, value, color);

            retDictionary.Add(id, e);
        }

        enemies = retDictionary;

        return retDictionary;
    }

    /// <summary>
    /// Returns Dictionary of Tower types from XML file
    /// 
    /// Authors: Cole Twitchell, Courtney Chu
    /// </summary>
    /// <returns>Dictionary of tower types</returns>
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
            string name = node.SelectSingleNode("name").InnerText;
            int cost = int.Parse(node.SelectSingleNode("cost").InnerText);
            int upgradeCost = int.Parse(node.SelectSingleNode("upgrade-cost").InnerText);
            float upgradeCostScale = float.Parse(node.SelectSingleNode("upgrade-cost-scale").InnerText);
            int maxLevel = int.Parse(node.SelectSingleNode("max-level").InnerText);
            float range = float.Parse(node.SelectSingleNode("range").InnerText);
            ColorType color = (ColorType)ColorType.Parse(typeof(ColorType), node.SelectSingleNode("color").InnerText);
            string description = node.SelectSingleNode("description").InnerText;

            // Projectile tower specific
            float projectileDamage = float.Parse(node.SelectSingleNode("proj-damage").InnerText);
            float projectileSpeed = float.Parse(node.SelectSingleNode("proj-speed").InnerText);
            float attackRate = float.Parse(node.SelectSingleNode("attack-rate").InnerText);

            ProjectileTower tower = (GameObject.Instantiate(Resources.Load("Prefabs/Towers/TowerPrefabs/" + name), LevelManager.PrefabHolderParent) as GameObject).GetComponent<ProjectileTower>();
            tower.Initialize(name, cost, upgradeCost, upgradeCostScale, maxLevel, range, color, description, attackRate, projectileSpeed, projectileDamage);

            retDictionary.Add(name, tower);
        }

        foreach (XmlNode node in doc.SelectSingleNode("towers").SelectNodes("aoe-tower"))
        {
            string name = node.SelectSingleNode("name").InnerText;
            int cost = int.Parse(node.SelectSingleNode("cost").InnerText);
            int upgradeCost = int.Parse(node.SelectSingleNode("upgrade-cost").InnerText);
            float upgradeCostScale = float.Parse(node.SelectSingleNode("upgrade-cost-scale").InnerText);
            int maxLevel = int.Parse(node.SelectSingleNode("max-level").InnerText);
            float range = float.Parse(node.SelectSingleNode("range").InnerText);
            ColorType color = (ColorType)ColorType.Parse(typeof(ColorType), node.SelectSingleNode("color").InnerText);
            string description = node.SelectSingleNode("description").InnerText;

            // Projectile tower specific
            float aoeDamage = float.Parse(node.SelectSingleNode("aoe-damage").InnerText);
            float attackRate = float.Parse(node.SelectSingleNode("attack-rate").InnerText);

            AoETower tower = (GameObject.Instantiate(Resources.Load("Prefabs/Towers/TowerPrefabs/" + name), LevelManager.PrefabHolderParent) as GameObject).GetComponent<AoETower>();
            tower.Initialize(name, cost, upgradeCost, upgradeCostScale, maxLevel, range, color, description, attackRate, aoeDamage);

            retDictionary.Add(name, tower);
        }

        foreach (XmlNode node in doc.SelectSingleNode("towers").SelectNodes("dot-tower"))
        {
            string name = node.SelectSingleNode("name").InnerText;
            int cost = int.Parse(node.SelectSingleNode("cost").InnerText);
            int upgradeCost = int.Parse(node.SelectSingleNode("upgrade-cost").InnerText);
            float upgradeCostScale = float.Parse(node.SelectSingleNode("upgrade-cost-scale").InnerText);
            int maxLevel = int.Parse(node.SelectSingleNode("max-level").InnerText);
            float range = float.Parse(node.SelectSingleNode("range").InnerText);
            ColorType color = (ColorType)ColorType.Parse(typeof(ColorType), node.SelectSingleNode("color").InnerText);
            string description = node.SelectSingleNode("description").InnerText;

            // Projectile tower specific
            float doTDamage = float.Parse(node.SelectSingleNode("dot-damage").InnerText);

            DoTTower tower = (GameObject.Instantiate(Resources.Load("Prefabs/Towers/TowerPrefabs/" + name), LevelManager.PrefabHolderParent) as GameObject).GetComponent<DoTTower>();
            tower.Initialize(name, cost, upgradeCost, upgradeCostScale, maxLevel, range, color, description, doTDamage);

            retDictionary.Add(name, tower);
        }

        towers = retDictionary;

        return retDictionary;
    }

    /// <summary>
    /// Removes all enemies and towers from their respective lists and sets these lists to null to allow for clean restarts
    /// </summary>
    public static void Cleanup()
    {
        foreach (Enemy e in enemies.Values)
        {
            if (e != null)
            {
                UnityEngine.MonoBehaviour.Destroy(e);
            }
        }

        foreach (Tower t in towers.Values)
        {
            if (t != null)
            {
                UnityEngine.MonoBehaviour.Destroy(t);
            }
        }

        waves = null;
        enemies = null;
        towers = null;
    }
}
