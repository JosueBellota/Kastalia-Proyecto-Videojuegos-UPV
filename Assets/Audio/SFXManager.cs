using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SFXManager : MonoBehaviour
{
    public GameObject sfxPrefab;
    public AudioClip clickClip;
    public AudioClip hoverClip;
    public string targetTag = "BotonUI";
    private static SFXManager instance;

    [Header("SFX de combate")]
    public AudioClip disparoLigeroSFX;
    public AudioClip disparoPesadoSFX;
    public AudioClip enemyWoundedSFX;
    public AudioClip playerWoundedSFX;
    public AudioClip swordSFX;

    public AudioClip swordHeavySFX;

    public AudioClip explosionSFX;
    public AudioClip runningSFX;

    public AudioClip PlayerDeathSFX;

    public AudioClip DemonSFX;

    public AudioClip DemonDamageSFX;

    public AudioClip FireballSFX;

    public AudioClip ForceFieldSFX;

    public AudioClip CuracionSFX;

    public AudioClip PickupSFX;

    private PlaySFX combateSFXPlayer;
    

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Crear instancia persistente para sonidos de combate
        if (sfxPrefab != null)
        {
            GameObject combateSFXObject = Instantiate(sfxPrefab, transform);
            combateSFXObject.name = "SFXCombate";
            combateSFXPlayer = combateSFXObject.GetComponent<PlaySFX>();

            if (combateSFXPlayer != null)
            {
                combateSFXPlayer.disparoLigeroClip = disparoLigeroSFX;
                combateSFXPlayer.disparoPesadoClip = disparoPesadoSFX;
                combateSFXPlayer.enemywoundedClip = enemyWoundedSFX;
                combateSFXPlayer.playerwoundedClip = playerWoundedSFX;
                combateSFXPlayer.swordClip = swordSFX;
                combateSFXPlayer.swordHeavyClip = swordHeavySFX;
                combateSFXPlayer.explosionClip = explosionSFX;
                combateSFXPlayer.runningClip = runningSFX;
                combateSFXPlayer.PlayerDeathClip = PlayerDeathSFX;
                combateSFXPlayer.DemonClip = DemonSFX;
                combateSFXPlayer.DemonDamageClip = DemonDamageSFX;
            }
        }
    }

    public void ReproducirDisparoLigero()
    {
        combateSFXPlayer?.Play(SFXType.Ligero);
    }

    public void ReproducirDisparoPesado()
    {
        combateSFXPlayer?.Play(SFXType.Pesado);
    }

    public void ReproducirEnemyWounded()
    {
        combateSFXPlayer?.Play(SFXType.EnemyWounded);
    }

    public void ReproducirPlayerWounded()
    {
        combateSFXPlayer?.Play(SFXType.PlayerWounded);
    }

    public void ReproducirSword()
    {
        combateSFXPlayer?.Play(SFXType.Sword);
    }

    public void ReproducirSwordHeavy()
    {
        combateSFXPlayer?.Play(SFXType.SwordHeavy);
    }
    
    public void ReproducirRunning()
    {
        combateSFXPlayer?.Play(SFXType.Running);
    }

    public void EmpezarRunningLoop()
    {
        combateSFXPlayer?.PlayRunningLoop();
    }

    public void DetenerRunningLoop()
    {
        combateSFXPlayer?.StopRunningLoop();
    }

    public void ReproducirExplosion()
    {
        combateSFXPlayer?.Play(SFXType.Explosion);
    }

    public void ReproducirPlayerDeath()
    {
        combateSFXPlayer?.Play(SFXType.PlayerDeath);
    }

    public void ReproducirDemon()
    {
        combateSFXPlayer?.Play(SFXType.Demon);
    }

    public void ReproducirDemonDamage()
    {
        combateSFXPlayer?.Play(SFXType.DemonDamage);
    }



    public void ReproducirCuracion()
    {
        if (CuracionSFX != null)
            combateSFXPlayer?.PlayOneShot(CuracionSFX);
    }

    public void ReproducirPickup()
    {
        if (PickupSFX != null)
            combateSFXPlayer?.PlayOneShot(PickupSFX);
    }

    public void ReproducirFireball()
    {
        if (FireballSFX != null)
            combateSFXPlayer?.PlayOneShot(FireballSFX);
    }

    public void ReproducirForceField()
    {
        if (ForceFieldSFX != null)
            combateSFXPlayer?.PlayOneShot(ForceFieldSFX);
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddSFXToTaggedButtons();
    }

    void AddSFXToTaggedButtons()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in taggedObjects)
        {
            Button btn = obj.GetComponent<Button>();
            if (btn != null)
            {
                GameObject sfxInstance = Instantiate(sfxPrefab, obj.transform);
                PlaySFX sfxPlayer = sfxInstance.GetComponent<PlaySFX>();

                if (sfxPlayer != null)
                {
                    sfxPlayer.clickClip = clickClip;
                    sfxPlayer.hoverClip = hoverClip;

                    btn.onClick.AddListener(() =>
                    {
                        SpecialSFXClip specialClip = obj.GetComponent<SpecialSFXClip>();
                        if (specialClip != null && specialClip.clickOverride != null)
                        {
                            sfxPlayer.PlayOneShot(specialClip.clickOverride);
                        }
                        else
                        {
                            sfxPlayer.Play(SFXType.Click);
                        }
                    });

                    EventTrigger trigger = obj.GetComponent<EventTrigger>();
                    if (trigger == null)
                        trigger = obj.AddComponent<EventTrigger>();

                    EventTrigger.Entry entry = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerEnter
                    };

                    entry.callback.AddListener((_) =>
                    {
                        sfxPlayer.Play(SFXType.Hover);
                    });

                    trigger.triggers.Add(entry);
                }
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static SFXManager GetInstance()
    {
        return instance;
    }
}
