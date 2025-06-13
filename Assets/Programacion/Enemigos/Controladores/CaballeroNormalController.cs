using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CaballeroNormalController : Maquina
{
    public EnemyType tipoEnemigo = EnemyType.CaballeroNormal;
    Transform jugador;
    public float distanciaAtaque = 3f;
    [SerializeField] float distanciaDeteccion = 5f;
    public float attackDamage = 10f;

    public NombreEstado deambularEstado;
    public NombreEstado perseguirEstado;
    public NombreEstado atacarEstado;

    public float AttackDistance => distanciaAtaque;
    public float DetectionDistance => distanciaDeteccion;
    public Transform Player => jugador;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agente;
    [HideInInspector] public float velocidadActual = 0f;
    [HideInInspector] public bool atacarAnimacion = false;

    private bool yaHaMuerto = false;

    void Start()
    {
        jugador = GameObject.FindWithTag("Player")?.transform;

        animator = GetComponentInChildren<Animator>();
        agente = GetComponent<NavMeshAgent>();

        if (animator == null) Debug.LogError("Caballero: Animator no encontrado.");
        if (agente == null) Debug.LogError("Caballero: NavMeshAgent no encontrado.");

        if (deambularEstado != null)
        {
            SetEstado(deambularEstado.Value);
        }
    }

    void Update()
    {
        if (!jugador)
        {
            jugador = GameObject.FindWithTag("Player")?.transform;
            if (!jugador) return;
        }

        // Animación de caminar
        if (animator != null)
        {
            animator.SetFloat("Speed", velocidadActual);

            if (atacarAnimacion)
            {
                animator.SetTrigger("Atacar");
                atacarAnimacion = false;
            }
        }
    }

    public void Morir()
    {
        if (!yaHaMuerto)
        {
            animator.SetTrigger("Morir");
            yaHaMuerto = true;
            StartCoroutine(EsperarYDesaparecer());
        }
    }

    IEnumerator EsperarYDesaparecer()
    {
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false); // o Destroy(gameObject);
    }
}
