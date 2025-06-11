using System.Collections;
using UnityEngine;

public class Arquero : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject prefabFlecha;
    public float fuerza = 30f;
    public float delayDisparoVisual = 0.5f; 
    public float delayRafaga = 0.05f;
    private float cooldownLigero = 0f;
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

        puedeDisparar = false; // ← bloqueo inmediato
        playerController.animator.SetTrigger("PrimaryShot");
        StartCoroutine(DispararConDelay(1, cooldownLigero, delayDisparoVisual ));
    }

    public void EmpezarCarga()
    {
        acumuladoCarga += Time.deltaTime;

        if (acumuladoCarga >= tiempoCarga && !cargando)
        {
            cargando = true;
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

        yield return new WaitForSeconds(cooldown);
        puedeDisparar = true;
    }
}
