using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public GameObject musicaPrefab; 
    public AudioClip menuClip;  
    public AudioClip mazmorraClip; 
    public AudioClip victoriaClip;
    public AudioClip derrotaClip;  

    private static MusicManager instance;
    private PlayMusica musicaPlayer;
    private AudioSource audioSource;

    // private bool wasPaused = false;



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

    void Update()
    {
        
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "PauseMenu")
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
        else
        {
            if (!audioSource.isPlaying && audioSource.clip != null)
                audioSource.UnPause();
        }
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

        if (sceneName == "MainMenu" || sceneName == "MenuSelection" || sceneName == "MenuOpciones" || sceneName == "MenuControles")
        {
            ChangeMusic(menuClip);
        }
        else if (sceneName.StartsWith("Mazmorra") || sceneName.StartsWith("Tutorial") || sceneName == "PauseMenu")
        {
            ChangeMusic(mazmorraClip);
        }

        else if (sceneName.StartsWith("Menu_Victoria"))
        {
            ChangeMusic(victoriaClip);
        }

        else if (sceneName.StartsWith("Derrota"))
        {
            ChangeMusic(derrotaClip);
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
