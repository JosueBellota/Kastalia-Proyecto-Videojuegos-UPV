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
    public AudioSource audioSource;
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
        AudioClip clipToPlay = null;

        switch (type)
        {
            case SFXType.Click:
                clipToPlay = clickClip;
                break;
            case SFXType.Hover:
                clipToPlay = hoverClip;
                break;
            case SFXType.Ligero:
                clipToPlay = disparoLigeroClip;
                break;
            case SFXType.Pesado:
                clipToPlay = disparoPesadoClip;
                break;
            case SFXType.EnemyWounded:
                clipToPlay = enemywoundedClip;
                break;
            case SFXType.PlayerWounded:
                clipToPlay = playerwoundedClip;
                break;
            case SFXType.Sword:
                clipToPlay = swordClip;
                break;
            case SFXType.Explosion:
                clipToPlay = explosionClip;
                break;

            case SFXType.Running:
                clipToPlay = runningClip;
                break;
            
        }

        if (clipToPlay != null)
        {
            PlaySimultaneous(clipToPlay);
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlaySimultaneous(AudioClip clip)
    {
        GameObject tempGO = new GameObject("TempSFX");
        tempGO.transform.SetParent(transform); // keep hierarchy organized
        AudioSource tempAudio = tempGO.AddComponent<AudioSource>();
        tempAudio.clip = clip;
        tempAudio.Play();
        Destroy(tempGO, clip.length);
    }
}
