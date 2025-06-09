using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SFXManager : MonoBehaviour
{
    public GameObject sfxPrefab; // Solo 1 prefab con PlaySFX
    public AudioClip clickClip;
    public AudioClip hoverClip;
    public string targetTag = "BotonUI";

    private static SFXManager instance;

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
                        sfxPlayer.Play(SFXType.Click);
                    });

                    // Hover
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
}
