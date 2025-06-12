using System.Collections.Generic;
using UnityEngine;

public class CofreItemDrop : MonoBehaviour
{
    public List<GameObject> posiblesItems;
    public Transform puntoSpawn;
    public float tiempoDeVida = 5f;

    private float tiempoSpawn;
    private bool jugadorCerca = false;
    private bool haSoltado = false;

    private Animator animator;

    void Start()
    {
        tiempoSpawn = Cronometro.instance != null ? Cronometro.instance.ObtenerTiempoRaw() : 0f;
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogWarning("No se encontró el Animator en el Cofre.");
    }

    void Update()
    {
        if (jugadorCerca && !haSoltado && Input.GetKeyDown(KeyCode.J))
        {
            AbrirCofre();
        }

        if (!haSoltado && Cronometro.instance != null)
        {
            float tiempoActual = Cronometro.instance.ObtenerTiempoRaw();
            if (tiempoActual >= tiempoSpawn + tiempoDeVida)
            {
                Destroy(gameObject);
            }
        }
    }

    void AbrirCofre()
    {
        if (animator != null)
            animator.SetTrigger("abrir");

        SoltarItem();
    }

    void SoltarItem()
    {
        if (posiblesItems.Count == 0)
        {
            Debug.LogWarning("No hay ítems asignados al cofre.");
            return;
        }

        int indice = Random.Range(0, posiblesItems.Count);
        GameObject seleccionado = posiblesItems[indice];

        Instantiate(seleccionado, puntoSpawn.position, Quaternion.identity);
        haSoltado = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }
}

