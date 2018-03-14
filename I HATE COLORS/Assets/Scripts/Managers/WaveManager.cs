using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages waves of enemies and loads which ones are supposed to be sent from wave_composition.xml.
/// </summary>
public class WaveManager : MonoBehaviour
{
    /// <summary>
    /// Boolean for if enemies are currently attacking or not.
    /// </summary>
    private static bool waveRunning = false;

    /// <summary>
    /// The file wave_composition.xml where the number and type of enemies are set.
    /// </summary>
    public static string waveXmlFile;

    /// <summary>
    /// The current wave of attacking enemies.
    /// </summary>
    private static Wave currentWave;
    public static Wave CurrentWave { get { return currentWave; } }

    /// <summary>
    /// A queue containing all waves for the game.
    /// </summary>
    private static Queue<Wave> waves;

    /// <summary>
    /// Instantiates the waves from the xml.
    /// </summary>
    public void Start()
    {
        waves = XmlImporter.GetWavesFromXml();
        WaveManager.SetNextWave();
    }

    /// <summary>
    /// Sets which waves should be running according to the xml.
    /// Edited by Courtney
    /// </summary>
    public static void Update()
    {
        if (currentWave != null && waveRunning)
        {
            currentWave.Update();

            if (currentWave.SpawnReady())
            {
                EnemyManager.SpawnEnemy(currentWave.NextEnemy());
            }
        }
        if (waves.Count == 0)
        {
            GameManager.gameOver = true;
        }
    }

    /// <summary>
    /// Gets the next wave from the queue.
    /// Edited by Courtney
    /// </summary>
    public static void SetNextWave()
    {
        currentWave = waves.Dequeue();
        waveRunning = false;
    }

    /// <summary>
    /// Clears all of the dead enemies from the field and starts the next wave.
    /// Edited by Courtney
    /// </summary>
    public static void BeginWave()
    {
        waveRunning = true;

        EnemyManager.ClearDeadEnemies();
    }

    /// <summary>
    /// Returns true if all of the enemies have either died or made it to the castle.
    /// Edited by Courtney
    /// </summary>
    public static bool WaveFinished()
    {
        return currentWave != null && currentWave.EnemiesRemaining() == 0;
    }

    /// <summary>
    /// Returns the queue holding all of the waves for the game.
    /// Edited by Courtney
    /// </summary>
    public static Queue<Wave> GetWaves()
    {
        return waves;
    }
}
