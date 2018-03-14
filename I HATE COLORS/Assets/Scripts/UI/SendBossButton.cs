using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button for sending a boss in the coming wave.
/// </summary>
public class SendBossButton : MonoBehaviour
{
    /// <summary>
    /// Button to send boss.
    /// </summary>
    private Button sendBossButton;

    /// <summary>
    /// True if button has been pressed for comming wave, false otherwise.
    /// </summary>
    public bool Active { get { return sendBossButton.IsInteractable(); } }

    /// <summary>
    /// Assign sendBossButton and reset button as a precaution.
    /// 
    /// Author: David Askari
    /// </summary>
    void Awake()
    {
        sendBossButton = gameObject.GetComponent<Button>();
        ResetButton();
    }

    /// <summary>
    /// Set button not interactable and enqueue boss to the coming wave.
    /// 
    /// Autor: David Askari
    /// </summary>
    public void SendBoss()
    {
        sendBossButton.interactable = false;
        Enemy boss = EnemyManager.enemyDictionary["boss"];
        WaveManager.CurrentWave.EnqueueEnemy(boss);
        AudioManager.PlayBossSentSound();
    }

    /// <summary>
    /// Set button interactable.
    /// 
    /// Autor: David Askari
    /// </summary>
    public void ResetButton()
    {
        sendBossButton.interactable = true;
    }

    /// <summary>
    /// Set button not interactable.
    /// 
    /// Author: David Askari
    /// </summary>
    public void DisableButton()
    {
        sendBossButton.interactable = false;
    }
}
