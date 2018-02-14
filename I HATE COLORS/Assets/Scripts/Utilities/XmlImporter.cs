using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XmlImporter {

    private static readonly string ENEMIES_XML_FILEPATH = "Resources/XML/enemies.xml";
    private static readonly string WAVE_COMPOSITION_FILEPATH = "Resources/XML/wave_composition.xml";

    private static Dictionary<string, Enemy> enemies = null;

    public static List<Wave> GetWavesFromXml()
    {
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
        
        return retList;
    }

    public static Dictionary<string, Enemy> GetEnemiesFromXml()
    {
        Dictionary<string, Enemy> retDictionary = new Dictionary<string, Enemy>();

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

            retDictionary.Add(id, e);
        }

        return retDictionary;
    }
}
