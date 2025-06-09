using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int characterIndex = -1;
    public bool playerSpawned = false;
    public bool isPaused = false;
    public bool isLevelLoaded = false;
    [SerializeField] public GameObject Lyx;
    [SerializeField] public GameObject Dreven;

    [SerializeField] private GameObject prefabHojaAfilada;
    [SerializeField] private GameObject prefabArco;

    public GameObject UI;
    public GameObject personajeSeleccionado;
    [SerializeField] private float fadeDuration = 0.25f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartMainMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    private void ResetGameState()
    {
        characterIndex = -1;
        personajeSeleccionado = null;
        playerSpawned = false;
        isPaused = false;
        isLevelLoaded = false;

        LevelManager.instance?.ResetLevelState(true);
        Cronometro.instance?.ReiniciarCronometro();
        ItemDropTracker.Reiniciar();
    }



    public void StartSeleccionMenu()
    {

        ResetGameState();

        StartCoroutine(LoadSceneWithTransition("MazmorraSelection", false));
    }
    public void StartMazmorraScene()
    {
        StartCoroutine(LoadSceneWithTransition("Mazmorra1", false));
    }

    public void PauseGame()
    {

        StartCoroutine(LoadSceneWithTransition("PauseMenu", true));
        Time.timeScale = 0f;
        isPaused = true;
    }


    public void Tutorial()
    {
        if (SceneManager.GetActiveScene().name == "Mazmorra1")
        {
            StartCoroutine(TutorialCoroutine());
        }
        }

    public void LoadTutorialScene(string sceneName, bool additive)
    {
        StartCoroutine(LoadSceneWithOutTransition(sceneName, additive));
    }

    private IEnumerator TutorialCoroutine()
    {
        yield return new WaitForSeconds(5f);

        // Verificamos nuevamente que el jugador sigue en la escena "Mazmorra1"
        if (SceneManager.GetActiveScene().name == "Mazmorra1")
        {
            StartCoroutine(LoadSceneWithOutTransition("Tutorial1", true));
        }
        else
        {
            Debug.Log("Tutorial cancelado: el jugador ya no está en Mazmorra1.");
        }

    }

    public void ResumeGame()
    {
        StartCoroutine(UnloadSceneWithTransition("PauseMenu"));
        Time.timeScale = 1f;
        isPaused = false;

    }

    public void SkipTutorial(string sceneToUnload)
    {
        StartCoroutine(UnloadSceneWithOutTransition(sceneToUnload));
    }

    public void StartMainMenu()
    {
        StartCoroutine(LoadSceneWithTransition("MainMenu", false));
    }

    public void WinGame()
    {
        isPaused = true;
        StartCoroutine(LoadSceneWithTransition("Menu_Victoria", true));
    }

    public void VolverAlMenuPrincipal()
    {
        ResetGameState();
        StartMainMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

   
    public void InstanciarArmaParaPersonaje()
    {
        if (LevelManager.instance == null || LevelManager.instance.player == null)
            return;

        GameObject playerInstance = LevelManager.instance.player;

        Vector3 posicionJugador = playerInstance.transform.position;
        Quaternion rotacionJugador = playerInstance.transform.rotation;

        if (playerInstance.name.Contains("Lyx") && prefabHojaAfilada != null)
        {
            Instantiate(prefabHojaAfilada, posicionJugador, rotacionJugador);
        }
        else if (playerInstance.name.Contains("Dreven") && prefabArco != null)
        {
            Instantiate(prefabArco, posicionJugador, rotacionJugador);
        }

        Tutorial();
    }

    public void SeleccionarPersonaje(GameObject personaje)
    {
        personajeSeleccionado = personaje;
        playerSpawned = true;

    }


    // -----------------------------------------------------------------------
    // -------------------Metodos Para Transiciones entre menus ----------------
    // -----------------------------------------------------------------------
    private IEnumerator LoadSceneWithTransition(string sceneName, bool additive)
    {
        GameObject fadeObject = CreateFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);
        var fadeObjectToDestroy = fadeObject;
        AsyncOperation asyncLoad;
        if (additive)
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.completed += (op) =>
            {
                if (fadeObjectToDestroy != null)
                    Destroy(fadeObjectToDestroy);
            };
        }

        yield return asyncLoad;
        if (!additive)
        {
            fadeObject = CreateFadeOverlay();
            fadeGroup = fadeObject.GetComponent<CanvasGroup>();
            fadeGroup.alpha = 1f; 
        }
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);
        if (fadeObject != null)
            Destroy(fadeObject);
    }



    private IEnumerator LoadSceneWithOutTransition(string sceneName, bool additive)
    {
        GameObject fadeObject = CreateSubtleFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        AsyncOperation asyncLoad = additive
            ? SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)
            : SceneManager.LoadSceneAsync(sceneName);

        yield return asyncLoad;
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);
        Destroy(fadeObject);
    }


    private IEnumerator UnloadSceneWithTransition(string sceneName)
    {
        GameObject fadeObject = CreateFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);
        yield return SceneManager.UnloadSceneAsync(sceneName);
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);
        Destroy(fadeObject);
    }

private IEnumerator UnloadSceneWithOutTransition(string sceneName)
{
    GameObject fadeObject = CreateSubtleFadeOverlay();
    CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();
    yield return Fade(fadeGroup, 0f, 1f, fadeDuration);
    var scene = SceneManager.GetSceneByName(sceneName);
    if (scene.IsValid() && scene.isLoaded)
    {
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        if (unloadOp != null)
        {
            yield return unloadOp;
        }
    }
    else
    {
        Debug.LogWarning($"Scene '{sceneName}' is not valid or not loaded. Skipping unload.");
    }
    yield return Fade(fadeGroup, 1f, 0f, fadeDuration);
    Destroy(fadeObject);
}

    private GameObject CreateFadeOverlay()
    {
        GameObject fadeObject = new GameObject("FadeOverlay");

        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; 

        CanvasGroup group = fadeObject.AddComponent<CanvasGroup>();

        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(fadeObject.transform);
        UnityEngine.UI.Image image = imageObject.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;

        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return fadeObject;
    }

    private GameObject CreateSubtleFadeOverlay()
    {
        GameObject fadeObject = new GameObject("SubtleFadeOverlay");

        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;

        CanvasGroup group = fadeObject.AddComponent<CanvasGroup>();

        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(fadeObject.transform);
        UnityEngine.UI.Image image = imageObject.AddComponent<UnityEngine.UI.Image>();

        image.color = new Color(0f, 0f, 0f, 0.2f); 

        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return fadeObject;
    }
    private IEnumerator Fade(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (group == null) yield break;
            float t = elapsed / duration;
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.SmoothStep(0f, 1f, t));
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        group.alpha = endAlpha;
    }
}