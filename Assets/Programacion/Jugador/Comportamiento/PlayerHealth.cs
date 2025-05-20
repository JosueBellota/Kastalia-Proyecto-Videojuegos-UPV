using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float vidaMaxima = 100f;
    public float vidaActual;
    private float tiempoInmunidad = .75f;
    private bool inmunidad = false;
    public int defensiveAbilityHits = 0;

    private PlayerController playerController;
    private MainInterface mainInterface;

    [SerializeField] private float transitionDuration = 0.25f;

    [SerializeField] private float fadeDuration = 0.25f;

    void Start()
    {
        vidaActual = vidaMaxima;
        playerController = GetComponent<PlayerController>();
        mainInterface = FindFirstObjectByType<MainInterface>();
    }

    public void takeDamage(float damage)
    {
        if (inmunidad || playerController.isDashing) return;
        if (defensiveAbilityHits > 0)
        {
            defensiveAbilityHits--;
            if (defensiveAbilityHits == 0) playerController.ToggleShieldPrefab(false);
            StartCoroutine(ActivarInmunidad());
            return;
        }
        if (vidaActual - damage > 0)
        {
            vidaActual -= damage;
            mainInterface.updateVidaText(vidaActual);
            StartCoroutine(ActivarInmunidad());
        }
        else
        {
            vidaActual = 0;
            Die();
        }
    }

    public void healPlayer(int ammount)
    {
        if (vidaActual <= 0) return;
        if (vidaActual + ammount > vidaMaxima) { vidaActual = vidaMaxima; }
        else { vidaActual += ammount; }

        mainInterface.updateVidaText(vidaActual);
    }

    public void Die()
    {
        if (Cronometro.instance != null)
        {
            Cronometro.instance.Detener();
        }

        mainInterface.updateVidaText(0f);

        StopAllCoroutines();
        LevelManager.instance.UI.SetActive(false);

        // Mostrar cursor del sistema
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Freeze game time
        GameManager.instance.isPaused = true;
        Time.timeScale = 0f;


        // Set animator to unscaled time
        playerController.animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        // Trigger death animation
        playerController.animator.SetTrigger("Death");

        // Start coroutine to wait for animation and load scene
        StartCoroutine(WaitForDeathAnimation());
    }

    IEnumerator ActivarInmunidad()
    {
        inmunidad = true;
        yield return new WaitForSeconds(tiempoInmunidad);
        inmunidad = false;
    }

    private IEnumerator WaitForDeathAnimation()
    {
        Animator animator = playerController.animator;

        // Wait until the animator is in the Death state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            yield return null;

        // Wait until animation finishes (using unscaled time)
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        float timer = 0f;

        while (timer < animationLength)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        // Load defeat scene additively
        StartCoroutine(LoadSceneWithTransition("Derrota", true));

        // Optionally destroy the player here or after scene finishes loading
        // Destroy(gameObject);
    }


    // --------------------------------------------------------------------------
    // ------------------- Metodos Para Transiciones entre menus ----------------
    // --------------------------------------------------------------------------
    // Hecho por JOSUE BELLOTA ICHASO (todo es culpa de el)
    

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

    // Helper method to handle fading
    private IEnumerator Fade(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        group.alpha = endAlpha;
    }   

}
