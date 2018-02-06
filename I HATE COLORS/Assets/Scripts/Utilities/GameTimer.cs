using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer {

    private bool isPaused;

    private float timeRemaining, lastTimeSet;

	// Use this for initialization
	void Start () {
        isPaused = false;
        timeRemaining = lastTimeSet = 0;
	}
	
	public void Update () {
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
        isPaused = !paused;
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
