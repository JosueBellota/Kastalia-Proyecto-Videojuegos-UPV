using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusica : MonoBehaviour
{

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void Toggle()
    {
        if (audioSource.isPlaying)
        {
            Stop();
        }
        else
        {
            Play();
        }
    }

    public void Toggle3D()
    {
        if(transform.parent == null)
        {
            transform.parent = FindAnyObjectByType<Rotator>().transform;
            audioSource.spatialBlend = 1f;
        }
        else
        {
            transform.parent = null;
            audioSource.spatialBlend = 0f;
        }
    }
}
