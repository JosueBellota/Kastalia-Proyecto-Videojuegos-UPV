    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(SphereCollider))]
    public class CofreItemDrop : MonoBehaviour
    {
        public List<GameObject> posiblesItems;
        public Transform puntoSpawn;
        // public float tiempoDeVida = 5f;
        // private float tiempoSpawn;
        private bool jugadorCerca = false;
        private bool haSoltado = false;

        private Animator animator;

        void Start()
        {
            // Tiempo de aparición
            // tiempoSpawn = Cronometro.instance != null ? Cronometro.instance.ObtenerTiempoRaw() : 0f;

            // Configurar el collider como trigger y con radio similar al wall
            SphereCollider sc = GetComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 3f; // Igual que wall

            animator = GetComponentInChildren<Animator>();
            if (animator == null)
                Debug.LogWarning("No se encontró el Animator en el Cofre.");
        }

        void Update()
        {
            if (jugadorCerca && !haSoltado && Input.GetKeyDown(KeyCode.J))
            {

                ///animacion caminzar
                Debug.Log("Presionaste J cerca del cofre. Intentando abrir...");
                AbrirCofre();
            }

            // if (!haSoltado && Cronometro.instance != null)
            // {
            //     float tiempoActual = Cronometro.instance.ObtenerTiempoRaw();
            //     if (tiempoActual >= tiempoSpawn + tiempoDeVida)
            //     {
            //         Destroy(gameObject);
            //     }
            // }
        }

        void AbrirCofre()
        {
        SFXManager.GetInstance()?.ReproducirOpenChest();
            if (animator != null)
            {
                animator.SetTrigger("abrir");
                Debug.Log("Animación de abrir cofre disparada.");
            }

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
            Debug.Log("Ítem soltado: " + seleccionado.name);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                jugadorCerca = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                jugadorCerca = false;
            }
        }
    }
