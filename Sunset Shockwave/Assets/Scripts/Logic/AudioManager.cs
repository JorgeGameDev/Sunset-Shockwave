using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used for audio or something.
public class AudioManager : MonoBehaviour {

    public static AudioManager audioManager;

	// Use this for initialization
	void Start ()
    {
        // Garants the music looping through.
	    if(audioManager == null)
        {
            audioManager = this;
        }	
        else
        {
            Destroy(gameObject);
        }
	}
}
