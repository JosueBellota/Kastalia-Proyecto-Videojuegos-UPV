using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float vidaMaxima = 100f;
    public float vidaActual;
    private float tiempoInmunidad = .75f;
    private bool inmunidad = false;
    public int defensiveAbilityHits = 0;
    private PlayerController playerController;
    private MainInterface mainInterface;
    private DamageFlash damageFlash;


    void Start()
    {

        damageFlash = GetComponent<DamageFlash>();

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
            damageFlash?.Flash();

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

        // Detener input y lógica de juego
        GameManager.instance.isPaused = true;
        Time.timeScale = 0f;

        // Preparar animator para reproducir animación sin interferencias
        Animator anim = playerController.animator;
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        anim.Rebind(); // Resetea el Animator completamente
        anim.Update(0f);

        // Desactivar cualquier estado conflictivo del player
        playerController.isDashing = false;
        playerController.isAttacking = false;
        playerController.isCastingAbility = false;
        playerController.ShowWeapon(false);
        playerController.ToggleShieldPrefab(false);

        // Desactivar movimiento
        if (playerController.GetComponent<CharacterController>())
            playerController.GetComponent<CharacterController>().enabled = false;

        // Reproducir animación de muerte directamente
        anim.Play("Death", 0, 0f);

        // Esperar animación antes de cargar escena
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

         GameManager.instance.StartDeathMenu();
    }

}
