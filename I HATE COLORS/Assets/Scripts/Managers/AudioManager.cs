using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource _source;
    private static AudioSource source;
    public AudioClip[] waveSongs;
    public static AudioClip beginWaveSound;

    int currentlyPlaying = 0;

    private void Start()
    {
        source = _source;
        beginWaveSound = Resources.Load<AudioClip>("Audio/unicornBeginWaveSound");
    }

    // Update is called once per frame
    void Update () {
		if (!source.isPlaying)
        {
            currentlyPlaying++;
            source.PlayOneShot(waveSongs[currentlyPlaying % waveSongs.Length]);
        }
	}

    public static void PlayBeginWaveSound()
    {
        source.PlayOneShot(beginWaveSound);
    }
}
