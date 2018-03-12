using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource source;
    public AudioClip[] waveSongs;

    int currentlyPlaying = 1;
	
	// Update is called once per frame
	void Update () {
		if (!source.isPlaying)
        {
            currentlyPlaying = currentlyPlaying == 1 ? 0 : 1;
            source.PlayOneShot(waveSongs[currentlyPlaying]);
        }
	}
}
