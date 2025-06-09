using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public GameObject musicaPrefab; 
    public AudioClip menuClip;  
    public AudioClip mazmorraClip;  

    private static MusicManager instance;
    private PlayMusica musicaPlayer;
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton para que persista entre escenas
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SetupMusica();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void SetupMusica()
    {
        if (musicaPlayer == null && musicaPrefab != null)
        {
            GameObject musicaInstance = Instantiate(musicaPrefab);
            DontDestroyOnLoad(musicaInstance);
            musicaPlayer = musicaInstance.GetComponent<PlayMusica>();
            audioSource = musicaInstance.GetComponent<AudioSource>();
            musicaPlayer.Init(); // 👈 Aquí inicializamos el audio source
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleMusicForScene(scene.name);
    }

    void HandleMusicForScene(string sceneName)
    {
        if (musicaPlayer == null || audioSource == null) return;

        if (sceneName == "MainMenu" || sceneName == "MenuSelection")
        {
            ChangeMusic(menuClip);
        }
        else if (sceneName.StartsWith("Mazmorra"))
        {
            ChangeMusic(mazmorraClip);
        }
        else
        {
            musicaPlayer.Stop();
        }
    }

    void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip && audioSource.isPlaying)
            return;

        musicaPlayer.Stop();
        audioSource.clip = newClip;
        musicaPlayer.Play();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
