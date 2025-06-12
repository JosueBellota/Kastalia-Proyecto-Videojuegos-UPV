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

    SwordHeavy,
    Explosion,

    Running,

    PlayerDeath,

    Demon,

    DemonDamage,

    Curacion,

    Fireball,

    ForceField,

    Pickup,

    Chest


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

    public AudioClip swordHeavyClip;

    public AudioClip explosionClip;
    public AudioClip runningClip;
    public AudioClip PlayerDeathClip;
    public AudioClip DemonClip;
    public AudioClip DemonDamageClip;

    public AudioClip FireballClip;

    public AudioClip ForceFieldClip;

    public AudioClip CuracionClip;

    public AudioClip PickupClip;

    public AudioClip ChestClip;

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
            case SFXType.SwordHeavy: clip = swordHeavyClip; break;
            case SFXType.Explosion: clip = explosionClip; break;
            case SFXType.PlayerDeath: clip = PlayerDeathClip; break;
            case SFXType.Demon: clip = DemonClip; break;
            case SFXType.DemonDamage: clip = DemonDamageClip; break;
            case SFXType.Fireball: clip = FireballClip; break;
            case SFXType.ForceField: clip = ForceFieldClip; break;
            case SFXType.Curacion: clip = CuracionClip; break;
            case SFXType.Pickup: clip = PickupClip; break;
            case SFXType.Chest: clip = ChestClip; break;



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

