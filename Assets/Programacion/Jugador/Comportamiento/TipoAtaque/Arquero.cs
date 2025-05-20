using System.Collections;
using UnityEngine;

public class Arquero : MonoBehaviour
{
    public GameObject prefabFlecha;
    public float fuerza = 30f;
    public float delayDisparo = 0.1f;
    public float delayRafaga = 0.05f;

    private float cooldownLigero = 0.5f;
    private float cooldownCargado = 1f;
    private float tiempoCarga = 1f;

    private float acumuladoCarga = 0f;
    private bool cargando = false;
    private bool puedeDisparar = true;


    private PosicionCursor posicionCursor;
    private PlayerInventory playerInventory;
    private EnemyLookAtSystem enemyLock;
    private PlayerController playerController;
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        posicionCursor = GetComponent<PosicionCursor>();
        playerInventory = GetComponent<PlayerInventory>();
        enemyLock = GetComponent<EnemyLookAtSystem>();
    }
    public void DisparoLigero()
    {
        if (!puedeDisparar || GameManager.instance.isPaused) return;
        playerController.animator.SetTrigger("PrimaryShot");
        StartCoroutine(Disparar(1, cooldownLigero, delayDisparo));
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

    IEnumerator Disparar(int cantidad, float cooldown, float delay)
    {
        puedeDisparar = false;
        Vector3 direction;
        if(enemyLock.isLocked && enemyLock.currentTarget) direction = (enemyLock.currentTarget.position + Vector3.down - transform.position).normalized;
        else direction = (posicionCursor.lookPoint - transform.position).normalized;

        for (int i = 0; i < cantidad; i++)
        {
            yield return new WaitForSeconds(delay);

            // Calculate spawn position for each arrow individually
            Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;

            // Spread logic
            Quaternion baseRotation = Quaternion.LookRotation(direction);
            if (cantidad > 1)
            {
                float spread = 5f;
                float offset = (i - (cantidad - 1) / 2f) * spread;
                baseRotation *= Quaternion.Euler(0, offset, 0);
            }

            GameObject flecha = Instantiate(prefabFlecha, spawnPos, baseRotation);
            flecha.GetComponent<Arrow>().setDamage(playerInventory.weapon.damage);
            Rigidbody rb = flecha.GetComponent<Rigidbody>();
            rb.AddForce(baseRotation * Vector3.forward * fuerza, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(cooldown);
        puedeDisparar = true;
    }

}
