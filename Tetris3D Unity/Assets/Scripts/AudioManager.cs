using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public AudioClip rowClearedFx;
    public AudioClip gameOverFx;
    public AudioClip groupLockFx;

    private AudioSource source;

    private void Start() {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public static void PlayAudioClip(AudioClip clip) {
        instance.source.PlayOneShot(clip);
    }
}
