using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Options menu for the game.
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    /// <summary>
    /// Whether audio is muted or not.
    /// </summary>
    private bool audioMuted;

    /// <summary>
    /// Text field of mute audio button.
    /// </summary>
    private Text muteAudioText;

    /// <summary>
    /// Find muteAudioText and set audioMuted to default setting (false).
    /// 
    /// Author: David Askari
    /// </summary>
    private void Start()
    {
        muteAudioText = transform.GetChild(1).GetComponentInChildren<Text>();
        audioMuted = false;
    }

    /// <summary>
    /// Resume game by changing Status of SumPause. Doing so will disable this menu and resume the game.
    /// 
    /// Author: David Askari
    /// </summary>
    public void ResumeGame()
    {
        PauseGame.Status = false;
    }

    /// <summary>
    /// Cleans up XML lists, sets paused status to false, loads main menu
    /// 
    /// Autor: David Askari, Courtney Chu, Cole Twitchell
    /// </summary>
    public void RestartGame()
    {
        XmlImporter.Cleanup();

        PauseGame.Status = false;
        GameObject.Find("GameManager").GetComponent<SceneLoader>().LoadScene("main_menu");
    }

    /// <summary>
    /// Switch audio and text of audio mute button.
    /// 
    /// Author: David Askari
    /// </summary>
    public void SwitchAudioMuted()
    {
        if (audioMuted)
        {
            AudioListener.volume = 1f;
            muteAudioText.text = "Mute Audio";
        }
        else
        {
            AudioListener.volume = 0f;
            muteAudioText.text = "Unmute Audio";
        }
        audioMuted = !audioMuted;
    }

    /// <summary>
    /// Quit the game. Nothing fancy to see here.
    /// 
    /// Author: David Askari
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
