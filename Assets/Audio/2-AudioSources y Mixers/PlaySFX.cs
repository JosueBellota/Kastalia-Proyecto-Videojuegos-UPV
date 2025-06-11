using UnityEngine;

public enum SFXType
{
    Click,
    Hover,
    Ligero,
    Pesado
}   
public class PlaySFX : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickClip;
    public AudioClip hoverClip;
    public AudioClip disparoLigeroClip;
    public AudioClip disparoPesadoClip;
    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogWarning("No AudioSource found on " + gameObject.name + ". Adding one automatically.");
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }
    }


    public void Play(SFXType type)
    {
        switch (type)
        {
            case SFXType.Click:
                PlayOneShot(clickClip);
                break;
            case SFXType.Hover:
                PlayOneShot(hoverClip);
                break;
            case SFXType.Ligero:
                PlayOneShot(disparoLigeroClip);
                break;
            case SFXType.Pesado:
                PlayOneShot(disparoPesadoClip);
                break;
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
