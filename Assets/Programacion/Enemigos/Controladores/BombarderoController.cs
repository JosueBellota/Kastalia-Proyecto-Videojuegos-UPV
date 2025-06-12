using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BombarderoController : Maquina
{
    public float shootingDistance = 15f;
    public float safeDistance = 5f;
    public bool isFiring = false;
    public Transform jugador;
    public float distanciaAJugador;

    public NombreEstado deambularEstado;
    public NombreEstado mantenerDistanciaEstado;
    public NombreEstado atacarEstado;
    public Bomba bombaPrefab;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agente;
    [HideInInspector] public float velocidadActual = 0f;
    [HideInInspector] public bool estaLanzandoBomba = false;

    private bool yaHaMuerto = false;

    void Start()
    {
        jugador = FindFirstObjectByType<CharacterController>()?.transform;
        animator = GetComponentInChildren<Animator>();
        agente = GetComponent<NavMeshAgent>();

        if (animator == null) Debug.LogError("Animator no encontrado.");
        if (agente == null) Debug.LogError("NavMeshAgent no encontrado.");
    }

    void Update()
    {
        if (!jugador)
        {
            var character = FindFirstObjectByType<CharacterController>();
            if (character == null) return;
            jugador = character.transform;
        }

        distanciaAJugador = getDistanceToPlayer();

        if (animator != null)
        {
            animator.SetFloat("Speed", velocidadActual);
        }
    }

    private float getDistanceToPlayer()
    {
        return jugador ? Vector3.Distance(transform.position, jugador.position) : 0f;
    }

    public IEnumerator ShootBomba()
    {
        Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;
        Vector3 targetPos = jugador.position + Vector3.up * 0.5f;

        Bomba bomba = Instantiate(bombaPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = bomba.GetComponent<Rigidbody>();

        float flightTime = 1.4f;
        Vector3 toTarget = targetPos - spawnPos;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);
        float yOffset = toTarget.y;
        float gravity = Mathf.Abs(Physics.gravity.y);

        float verticalVelocity = (yOffset + 0.5f * gravity * flightTime * flightTime) / flightTime;
        verticalVelocity *= 1.2f;

        Vector3 horizontalVelocity = toTargetXZ / flightTime;
        Vector3 launchVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

        rb.linearVelocity = launchVelocity;

        bomba.IniciarCuentaRegresiva();

        yield return new WaitForSeconds(0.1f); // espera mínima tras lanzar

        estaLanzandoBomba = false;

        // Recalcula estado tras lanzar
        if (distanciaAJugador <= safeDistance)
        {
            SetEstado(mantenerDistanciaEstado.Value);
        }
        else if (distanciaAJugador <= shootingDistance)
        {
            SetEstado(atacarEstado.Value);
        }
        else
        {
            SetEstado(deambularEstado.Value);
        }
    }

    public void LanzarBomba()
    {
        if (isFiring && !estaLanzandoBomba)
        {
            estaLanzandoBomba = true;
            StartCoroutine(ShootBomba());
            isFiring = false;
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
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
