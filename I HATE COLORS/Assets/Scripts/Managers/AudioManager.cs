using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all sound tracks and sound effects in the game.
/// 
/// Author: Cole Twitchell
/// </summary>
public class AudioManager : MonoBehaviour {

    /// <summary>
    /// Audio source to control all audio in our game.
    /// </summary>
    public AudioSource _source;
    private static AudioSource source;

    /// <summary>
    /// Array that contains all songs in our soundtrack.
    /// </summary>
    public AudioClip[] waveSongs;

    /// <summary>
    /// Cute sound effect that plays at the beginning of each wave of enemies.
    /// </summary>
    public static AudioClip beginWaveSound;

    /// <summary>
    /// Lighting sound that plays when the "Send Boss" button is clicked.
    /// </summary>
    public static AudioClip bossSentSound;

    /// <summary>
    /// Count to keep track of which song is playing in the soundtrack.
    /// </summary>
    int currentlyPlaying = 0;

    /// <summary>
    /// Initializes the AudioSource and sound effects in the game.
    ///  
    /// Authors: Amy Lewis, Cole Twitchell
    /// </summary>
    private void Start()
    {
        source = _source;
        beginWaveSound = Resources.Load<AudioClip>("Audio/unicornBeginWaveSound");
        bossSentSound = Resources.Load<AudioClip>("Audio/LightningSound");
    }

    /// <summary>
    /// Controls when to switch songs on our soundtrack.
    /// 
    /// Authors: Amy Lewis, Cole Twitchell
    /// </summary>
    void Update () {
		if (!source.isPlaying)
        {
            currentlyPlaying++;
            source.PlayOneShot(waveSongs[currentlyPlaying % waveSongs.Length]);
        }
	}

    /// <summary>
    /// Makes it possible to play the boss send sound anywhere in the game.
    /// Called in SendBossButton.cs
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public static void PlayBossSentSound()
    {
        source.PlayOneShot(bossSentSound);
    }

    /// <summary>
    /// Makes it possible to play the begin wave sound anywhere in the game.
    /// Called in GameManager.cs
    /// 
    /// Author: Amy Lewis
    /// </summary>
    public static void PlayBeginWaveSound()
    {
        source.PlayOneShot(beginWaveSound);
    }
}
