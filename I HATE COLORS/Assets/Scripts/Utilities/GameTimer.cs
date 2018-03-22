using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a generic timer. It is initialized, updated every frame, and can be reset at will.
/// 
/// Author: Cole Twitchell
/// </summary>
[System.Serializable]
public class GameTimer
{
    private bool isPaused;

    public float timeRemaining;
    public float lastTimeSet;

    // Blinking mechanics created and handled by Steven Johnson
    public bool startBlinking;

    /// <summary>
    /// Instantiates the game timer
    /// Author: Cole Twitchell
    /// </summary>
    public GameTimer() { }
    public GameTimer(float initialTime)
    {
        timeRemaining = lastTimeSet = initialTime;
        startBlinking = false;
    }

    /// <summary>
    /// Updates remaining time
    /// </summary>
    public void Update()
    {
        if (!isPaused)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Returns remaining time as a float
    /// </summary>
    /// <returns> remaining time as a float</returns>
    public float TimeRemaining()
    {
        return timeRemaining;
    }

    /// <summary>
    /// Sets time remaining as a float
    /// </summary>
    /// <param name="time">float</param>
    public void SetTimer(float time)
    {
        timeRemaining = lastTimeSet = time;
    }

    /// <summary>
    /// Sets the amount of time to be added to the timer after calling Reset()
    /// </summary>
    /// <param name="time"></param>
    public void SetResetTime(float time)
    {
        lastTimeSet = time;
    }

    /// <summary>
    /// Pauses or unpauses the game timer
    /// </summary>
    /// <param name="paused">bool</param>
    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    /// <summary>
    /// Sets remaining time to zero
    /// </summary>
    public void SkipTimer()
    {
        timeRemaining = 0;
    }

    /// <summary>
    /// Returns whether the game timer is paused
    /// </summary>
    /// <returns>bool game timer pause status</returns>
    public bool IsPaused()
    {
        return isPaused;
    }

    /// <summary>
    /// Resets remaining time to the last start time and ceases to blink
    /// </summary>
    public void Reset()
    {
        timeRemaining = lastTimeSet;
        startBlinking = false;
    }

    /// <summary>
    /// Returns whether the timer has expired.
    /// </summary>
    /// <returns>bool is time remaining zero</returns>
    public bool IsDone()
    {
        if(timeRemaining <= 0) {
            startBlinking = true;
        }
        return timeRemaining <= 0;
    }
}
