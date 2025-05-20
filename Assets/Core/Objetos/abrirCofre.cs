using System.Collections.Generic;
using UnityEngine;

public class CofreItemDrop : MonoBehaviour
{
    public List<GameObject> posiblesItems;
    public Transform puntoSpawn;

    public float tiempoDeVida = 5f; // segundos hasta desaparecer
    private float tiempoSpawn;

    private bool jugadorCerca = false;
    private bool haSoltado = false;

    void Start()
    {
        tiempoSpawn = Cronometro.instance != null ? Cronometro.instance.ObtenerTiempoRaw() : 0f;
    }

    void Update()
    {
        if (jugadorCerca && !haSoltado && Input.GetKeyDown(KeyCode.J))
        {
            SoltarItem();
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
