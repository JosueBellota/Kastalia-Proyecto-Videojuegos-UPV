using UnityEngine;

public enum SFXType
{
    Click,
    Hover,
    Ligero,
    Pesado,
    EnemyWounded,
    PlayerWounded,
    Sword,
    Explosion,

    Running
}

public class PlaySFX : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip clickClip;
    public AudioClip hoverClip;

    public AudioClip disparoLigeroClip;
    public AudioClip disparoPesadoClip;
    public AudioClip enemywoundedClip;
    public AudioClip playerwoundedClip;
    public AudioClip swordClip;
    public AudioClip explosionClip;
    public AudioClip runningClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(SFXType type)
    {
        AudioClip clip = null;
        switch (type)
        {
            case SFXType.Click: clip = clickClip; break;
            case SFXType.Hover: clip = hoverClip; break;
            case SFXType.Ligero: clip = disparoLigeroClip; break;
            case SFXType.Pesado: clip = disparoPesadoClip; break;
            case SFXType.EnemyWounded: clip = enemywoundedClip; break;
            case SFXType.PlayerWounded: clip = playerwoundedClip; break;
            case SFXType.Sword: clip = swordClip; break;
            case SFXType.Explosion: clip = explosionClip; break;
            case SFXType.Running: 
                PlayRunningLoop();
                return;
        }

        if (clip != null)
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlayRunningLoop()
    {
        if (audioSource.isPlaying && audioSource.clip == runningClip && audioSource.loop)
            return;

        audioSource.clip = runningClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopRunningLoop()
    {
        if (audioSource.isPlaying && audioSource.clip == runningClip)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
    }
}

