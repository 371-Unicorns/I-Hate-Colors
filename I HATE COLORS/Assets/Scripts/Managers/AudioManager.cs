using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource _source;
    private static AudioSource source;
    public AudioClip[] waveSongs;
    public static AudioClip beginWaveSound;

    int currentlyPlaying = 1;

    private void Start()
    {
        source = _source;
        beginWaveSound = Resources.Load<AudioClip>("Audio/unicornBeginWaveSound");
    }

    // Update is called once per frame
    void Update () {
		if (!source.isPlaying)
        {
            currentlyPlaying = currentlyPlaying == 1 ? 0 : 1;
            source.PlayOneShot(waveSongs[currentlyPlaying]);
        }
	}

    public static void PlayBeginWaveSound()
    {
        source.PlayOneShot(beginWaveSound);
    }
}
