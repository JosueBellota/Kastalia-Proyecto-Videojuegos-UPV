using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BallesteroController : Maquina
{
    public EnemyType tipoEnemigo = EnemyType.Ballestero;
    public float attackDamage = 20f;
    public float shootingDistance = 15f;
    public float safeDistance = 10f;
    public float fireCooldown = 1f;
    private float arrowForce = 20f;
    public bool isFiring = false;

    public Transform jugador;
    public float distanciaAJugador;

    public NombreEstado deambularEstado;
    public NombreEstado mantenerDistanciaEstado;
    public NombreEstado atacarEstado;
    public GameObject arrowPrefab;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agente;
    [HideInInspector] public float velocidadActual = 0f;
    [HideInInspector] public bool dispararAnimacion = false;

    private bool yaHaMuerto = false;

    void Start()
    {
        jugador = FindFirstObjectByType<CharacterController>()?.transform;
        animator = GetComponentInChildren<Animator>();
        agente = GetComponent<NavMeshAgent>();

        if (animator == null) Debug.LogError("Ballestero: Animator no encontrado.");
        if (agente == null) Debug.LogError("Ballestero: NavMeshAgent no encontrado.");
    }

    void Update()
    {
        if (!jugador)
        {
            jugador = FindFirstObjectByType<CharacterController>()?.transform;
            if (!jugador) return;
        }

        distanciaAJugador = getDistanceToPlayer();

        if (animator != null)
        {
            animator.SetFloat("Speed", velocidadActual);

            if (dispararAnimacion)
            {
                animator.SetTrigger("Atacar");
                dispararAnimacion = false;
            }
        }
    }

    private float getDistanceToPlayer()
    {
        return jugador ? Vector3.Distance(transform.position, jugador.position) : 0f;
    }

    public IEnumerator ShootArrow()
    {
        isFiring = true;

        dispararAnimacion = true; // ← activar trigger desde Update()

        Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;
        Vector3 direction = (jugador.position - transform.position).normalized;

        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.LookRotation(direction));
        arrow.GetComponent<Arrow>().setDamage(attackDamage);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(direction * arrowForce, ForceMode.Impulse);

        yield return new WaitForSeconds(fireCooldown);
        isFiring = false;
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
