﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimer
{
    private bool isPaused;

    public float timeRemaining;
    public float lastTimeSet;

    public void Update()
    {
        if (!isPaused)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    public float TimeRemaining()
    {
        return timeRemaining;
    }

    public void SetTimer(float time)
    {
        timeRemaining = lastTimeSet = time;
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    public void SkipTimer()
    {
        timeRemaining = 0;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void Reset()
    {
        timeRemaining = lastTimeSet;
    }

    public bool IsDone()
    {
        return timeRemaining <= 0;
    }
}
