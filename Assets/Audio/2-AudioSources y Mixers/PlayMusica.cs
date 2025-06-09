using UnityEngine;

public class PlayMusica : MonoBehaviour
{
    private AudioSource audioSource;

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if (audioSource != null)
            audioSource.Play();
    }

    public void Stop()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    public void Toggle()
    {
        if (audioSource != null)
        {
            if (audioSource.isPlaying)
                Stop();
            else
                Play();
        }
    }
}
