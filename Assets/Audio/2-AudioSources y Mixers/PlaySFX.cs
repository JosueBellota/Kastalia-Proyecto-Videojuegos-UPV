using UnityEngine;

public enum SFXType
{
    Click,
    Hover
}

public class PlaySFX : MonoBehaviour
{
    public AudioClip clickClip;
    public AudioClip hoverClip;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(SFXType type)
    {
        AudioClip selected = null;
        switch (type)
        {
            case SFXType.Click:
                selected = clickClip;
                break;
            case SFXType.Hover:
                selected = hoverClip;
                break;
        }

        if (selected != null)
        {
            audioSource.PlayOneShot(selected);
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

}
