using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioWaves : MonoBehaviour
{

    ParticleSystem waves;

    AudioSource audioSource;

    bool play;

    public bool Play
    {
        get => play;
        set
        {
            if (value == play) return;
            play = value;
            if (play)
            {
                waves.Play();
            }
            else
            {
                waves.Stop();
            }
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        waves = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Play = audioSource.isPlaying;
    }
}
