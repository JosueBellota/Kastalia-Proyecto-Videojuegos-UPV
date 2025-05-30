using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CtrlAudio : MonoBehaviour
{
    public AudioMixerSnapshot normalAudio;
    public AudioMixerSnapshot pausedAudio;
    public GameObject pauseMask;

    bool paused = false;

    public bool Paused
    {
        get => paused;
        set
        {
            if (paused == value) return;
            paused = value;
            if(paused)
            {
                pausedAudio.TransitionTo(1f);
                pauseMask.SetActive(true);
            }
            else
            {
                normalAudio.TransitionTo(1f);
                pauseMask.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
        }
    }
}
