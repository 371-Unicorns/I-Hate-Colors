using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage SettingsPanel with pauseResumeButton and settingsButton.
/// </summary>
public class SettingsPanel : Singleton<SettingsPanel>
{
    /// <summary>
    /// Button to pause/resume the game.
    /// </summary>
    private Button pauseResumeButton;

    /// <summary>
    /// Four sprites for pause.
    /// </summary>
    [SerializeField]
    private Sprite[] pauseButtonSprites;

    /// <summary>
    /// Four sprites for play.
    /// </summary>
    [SerializeField]
    private Sprite[] playButtonSprites;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private SettingsPanel() { }

    private void Start()
    {
        pauseResumeButton = transform.Find("PauseResumeButton").GetComponentInChildren<Button>();
    }

    /// <summary>
    /// Pause or resume the game, based on its current state.
    /// </summary>
    public void PauseResumeGame()
    {
        if (GameManager.Instance.gameRunning)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
        GameManager.Instance.gameRunning = !GameManager.Instance.gameRunning;
    }

    /// <summary>
    /// Pause the game and update pauseResumeButton to show play sprites.
    /// </summary>
    private void PauseGame()
    {
        Time.timeScale = 0.0f;

        SpriteState spriteState = new SpriteState();
        spriteState = pauseResumeButton.spriteState;
        spriteState.highlightedSprite = playButtonSprites[0];
        spriteState.pressedSprite = playButtonSprites[1];
        spriteState.disabledSprite = playButtonSprites[3];
        pauseResumeButton.spriteState = spriteState;

        pauseResumeButton.GetComponent<Image>().sprite = playButtonSprites[2];
    }

    /// <summary>
    /// Resume the game and update pauseResumeButton to show pause sprites.
    /// </summary>
    private void ResumeGame()
    {
        Time.timeScale = 1.0f;

        SpriteState spriteState = new SpriteState();
        spriteState = pauseResumeButton.spriteState;
        spriteState.highlightedSprite = pauseButtonSprites[0];
        spriteState.pressedSprite = pauseButtonSprites[1];
        spriteState.disabledSprite = pauseButtonSprites[3];
        pauseResumeButton.spriteState = spriteState;

        pauseResumeButton.GetComponent<Image>().sprite = pauseButtonSprites[2];
    }
}
