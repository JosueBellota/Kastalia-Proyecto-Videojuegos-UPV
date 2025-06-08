using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // revert
    public static GameManager instance;
    public int characterIndex = -1;
    public bool playerSpawned = false;
    public bool isPaused = false;
    public bool isLevelLoaded = false;


    [SerializeField] public GameObject Lyx;
    [SerializeField] public GameObject Dreven;

    // 🆕 NUEVO: Referencias a los prefabs de armas
    [SerializeField] private GameObject prefabHojaAfilada;
    [SerializeField] private GameObject prefabArco;

    public GameObject UI;

    public GameObject personajeSeleccionado;


    // [SerializeField] private float transitionDuration = 0.5f;

    [SerializeField] private float fadeDuration = 0.5f;

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

    public void StartMainGameLoop()
    {
        characterIndex = -1;
        personajeSeleccionado = null;
        playerSpawned = false;
        isLevelLoaded = false;

        if (LevelManager.instance != null)
            LevelManager.instance.ResetLevelState(true);
        if (LevelManager.instance != null)
            LevelManager.instance.ResetLevelState(true);

        // Limpiar cofres anteriores
        ItemDropTracker.Reiniciar();
        StartCoroutine(CargarMazmorraYSeleccion());
    }

    public void WinGame()
    {
        isPaused = true;
        StartCoroutine(LoadSceneWithTransition("Menu_Victoria", true));
    }

    private IEnumerator CargarMazmorraYSeleccion()
    {

        GameObject fadeObject = CreateFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();
        AsyncOperation loadMazmorra = SceneManager.LoadSceneAsync("Mazmorra1");
        yield return loadMazmorra;

        fadeObject = CreateFadeOverlay();
        fadeGroup = fadeObject.GetComponent<CanvasGroup>();
        fadeGroup.alpha = 1f; // Start from black
        AsyncOperation loadSelection = SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Additive);
        yield return loadSelection;
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);

        Destroy(fadeObject);
    }

    public void PauseGame()
    {

        StartCoroutine(LoadSceneWithTransition("PauseMenu", true));
        Time.timeScale = 0f;
        isPaused = true;
    }


    public void Tutorial()
    {
        StartCoroutine(TutorialCoroutine());
    }

    public void LoadTutorialScene(string sceneName, bool additive)
    {
        StartCoroutine(LoadSceneWithOutTransition(sceneName, additive));
    }

    private IEnumerator TutorialCoroutine()
    {
        yield return new WaitForSeconds(5f);

        StartCoroutine(LoadSceneWithOutTransition("Tutorial1", true));
        // Time.timeScale = 0f;
        // isPaused = true;
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
        // Time.timeScale = 1f;
        // isPaused = false;
    }

    public void StartMainMenu()
    {
        StartCoroutine(LoadSceneWithTransition("MainMenu", false));
    }

    public void VolverAlMenuPrincipal()
    {
        characterIndex = -1;
        personajeSeleccionado = null;
        playerSpawned = false;
        isPaused = false;
        isLevelLoaded = false;

        if (LevelManager.instance != null)
        {
            LevelManager.instance.ResetLevelState(true);
        }

        if (Cronometro.instance != null)
        {
            Cronometro.instance.ReiniciarCronometro();
        }

        // 🔁 Limpiar ítems de cofres al volver al menú principal
        ItemDropTracker.Reiniciar();

        StartCoroutine(LoadSceneWithTransition("MainMenu", false));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // 🆕 NUEVO: Método para instanciar el arma cerca del personaje seleccionado
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

    // 🆕 NUEVO: Método sugerido para ser llamado después de seleccionar personaje
    public void SeleccionarPersonaje(GameObject personaje)
    {
        personajeSeleccionado = personaje;
        playerSpawned = true;

        // InstanciarArmaParaPersonaje(); // 🆕 Instancia el arma correspondiente
    }


    // -----------------------------------------------------------------------
    // -------------------Metodos Para Transiciones entre menus ----------------
    // -----------------------------------------------------------------------

    private IEnumerator LoadSceneWithTransition(string sceneName, bool additive)
    {
        // Create a simple fade overlay
        GameObject fadeObject = CreateFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();

        // Fade in (to black)
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        // Store reference before potential destruction
        var fadeObjectToDestroy = fadeObject;

        // Load the target scene
        AsyncOperation asyncLoad;
        if (additive)
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            // When loading a new scene non-additively, our fade object will be destroyed
            // so we need to recreate it after scene load
            asyncLoad.completed += (op) =>
            {
                if (fadeObjectToDestroy != null)
                    Destroy(fadeObjectToDestroy);
            };
        }

        yield return asyncLoad;

        // For non-additive loads, recreate the fade object
        if (!additive)
        {
            fadeObject = CreateFadeOverlay();
            fadeGroup = fadeObject.GetComponent<CanvasGroup>();
            fadeGroup.alpha = 1f; // Start from black
        }

        // Fade out (to clear)
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);

        // Clean up
        if (fadeObject != null)
            Destroy(fadeObject);
    }



    private IEnumerator LoadSceneWithOutTransition(string sceneName, bool additive)
    {
        GameObject fadeObject = CreateSubtleFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();

        // Soft fade in
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        AsyncOperation asyncLoad = additive
            ? SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)
            : SceneManager.LoadSceneAsync(sceneName);

        yield return asyncLoad;

        // Soft fade out
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);

        Destroy(fadeObject);
    }


    private IEnumerator UnloadSceneWithTransition(string sceneName)
    {
        // Create a simple fade overlay
        GameObject fadeObject = CreateFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();

        // Fade in (to black)
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        // Unload the scene
        yield return SceneManager.UnloadSceneAsync(sceneName);

        // Fade out (to clear)
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);

        // Clean up
        Destroy(fadeObject);
    }

   private IEnumerator UnloadSceneWithOutTransition(string sceneName)
    {
        GameObject fadeObject = CreateSubtleFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();

        // Soft fade in
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        yield return SceneManager.UnloadSceneAsync(sceneName);

        // Soft fade out
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);

        Destroy(fadeObject);
    }


    // Helper method to create a simple fade overlay
    private GameObject CreateFadeOverlay()
    {
        GameObject fadeObject = new GameObject("FadeOverlay");

        // Setup Canvas
        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // Make sure it's on top

        // Setup CanvasGroup for fading
        CanvasGroup group = fadeObject.AddComponent<CanvasGroup>();

        // Create full-screen image
        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(fadeObject.transform);
        UnityEngine.UI.Image image = imageObject.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;

        // Stretch to full screen
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

        // Setup Canvas
        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;

        // Setup CanvasGroup
        CanvasGroup group = fadeObject.AddComponent<CanvasGroup>();

        // Create full-screen image
        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(fadeObject.transform);
        UnityEngine.UI.Image image = imageObject.AddComponent<UnityEngine.UI.Image>();

        // Use black with low alpha (subtle)
        image.color = new Color(0f, 0f, 0f, 0.2f); 

        // Stretch to full screen
        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return fadeObject;
    }


    // Helper method to handle fading
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