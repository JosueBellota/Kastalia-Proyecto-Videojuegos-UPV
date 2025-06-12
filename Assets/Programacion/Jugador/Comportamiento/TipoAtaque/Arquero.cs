using System.Collections;
using UnityEngine;

public class Arquero : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject prefabFlecha;
    public float fuerza = 30f;

    // delayRafaga: Tiempo entre cada flecha disparada en un ataque cargado (ráfaga múltiple).
    public float delayRafaga = 0.05f;

    // cooldownLigero: Tiempo que debe pasar después de un disparo ligero antes de poder volver a disparar.
    private float cooldownLigero = 0f;

    // cooldownCargado: Tiempo que debe pasar después de un disparo cargado antes de poder volver a disparar.
    private float cooldownCargado = 1f;


    [Header("Carga")]
    private float tiempoCarga = 1f;
    private float acumuladoCarga = 0f;
    private bool cargando = false;
    private bool puedeDisparar = true;

    // Referencias
    private PosicionCursor posicionCursor;
    private PlayerInventory playerInventory;
    private EnemyLookAtSystem enemyLock;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        posicionCursor = GetComponent<PosicionCursor>();
        playerInventory = GetComponent<PlayerInventory>();
        enemyLock = GetComponent<EnemyLookAtSystem>();
    }

    public void DisparoLigero()
    {
        if (!puedeDisparar || GameManager.instance.isPaused) return;

        // AQUI se lanza el sonido del disparo ligero
        SFXManager.GetInstance()?.ReproducirDisparoLigero();

        puedeDisparar = false;
        playerController.animator.SetTrigger("PrimaryShot");

        
        

        StartCoroutine(Disparar(1, cooldownLigero, 1f));
        
    }


    public void EmpezarCarga()
    {
        acumuladoCarga += Time.deltaTime;

        if (acumuladoCarga >= tiempoCarga && !cargando)
        {
              
            
            cargando = true;
            SFXManager.GetInstance()?.ReproducirDisparoPesado();
            playerController.animator.SetTrigger("SecondaryShot");


            
          

            StartCoroutine(Disparar(5, cooldownCargado, delayRafaga));
        }
    }

    public void SoltarCarga()
    {
        acumuladoCarga = 0f;
        cargando = false;
    }

    private IEnumerator DispararConDelay(int cantidad, float cooldown, float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(Disparar(cantidad, cooldown, 0.8f));
    }

    private IEnumerator Disparar(int cantidad, float cooldown, float delay)
    {
        Vector3 direction = enemyLock.isLocked && enemyLock.currentTarget
            ? (enemyLock.currentTarget.position + Vector3.down - transform.position).normalized
            : (posicionCursor.lookPoint - transform.position).normalized;

        for (int i = 0; i < cantidad; i++)
        {

            // delay entre cada flecha (solo afecta al disparo pesado)
            yield return new WaitForSeconds(delay); 

            Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;
            Quaternion baseRotation = Quaternion.LookRotation(direction);

            if (cantidad > 1)
            {
                float spread = 5f;
                float offset = (i - (cantidad - 1) / 2f) * spread;
                baseRotation *= Quaternion.Euler(0, offset, 0);
            }

            GameObject flecha = Instantiate(prefabFlecha, spawnPos, baseRotation);
            flecha.GetComponent<Arrow>().setDamage(playerInventory.weapon.damage);
            flecha.GetComponent<Rigidbody>().AddForce(baseRotation * Vector3.forward * fuerza, ForceMode.Impulse);
        }

        // Aquí se aplica el cooldown antes de volver a permitir disparar:
        yield return new WaitForSeconds(cooldown);

        // ← Este cooldown depende del tipo de disparo:
        // - DisparoLigero usa cooldownLigero = 0.5f
        // - Disparo cargado (pesado) usa cooldownCargado = 1f
        puedeDisparar = true;
    }

}
