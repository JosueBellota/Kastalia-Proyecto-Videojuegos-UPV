using System.Collections;
using UnityEngine;

public class BombarderoController : Maquina
{
    //Atributos relacionados al enemigo
    public float shootingDistance = 15f;
    public float safeDistance = 5f;
    // public float fireCooldown = 0f;
    public bool isFiring = false;

    //Atributos relacionados al jugador
    public Transform jugador;
    public float distanciaAJugador;
    
    //Estados
    public NombreEstado deambularEstado;
    public NombreEstado mantenerDistanciaEstado;
    public NombreEstado atacarEstado;
    public Bomba bombaPrefab;

    void Start()
    {
        if (FindFirstObjectByType<CharacterController>() == null) { return; }
        else { jugador = FindFirstObjectByType<CharacterController>().transform; }
    }

    void Update()
    {
        if (!jugador)
        {
            if (FindFirstObjectByType<CharacterController>() == null) return;
            else { jugador = FindFirstObjectByType<CharacterController>().transform; }
        }
        distanciaAJugador = getDistanceToPlayer();
        
        //   // NUEVA LÓGICA PARA DISPARO AUTOMÁTICO
        // if (!isFiring && distanciaAJugador <= shootingDistance)
        // {
        //     isFiring = true;
        //     StartCoroutine(ShootBomba());
        // }
    }

    private float getDistanceToPlayer(){
        if (jugador){
            return Vector3.Distance(transform.position, jugador.position);
        } else {
            Debug.LogError("No se ha encontrado al jugador.");
            return 0;
        }
    }

    public IEnumerator ShootBomba()
    {
        Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;
        Vector3 targetPos = jugador.position + Vector3.up * 0.5f;

        Bomba bomba = Instantiate(bombaPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = bomba.GetComponent<Rigidbody>();

        // Tiempo deseado en el aire — más alto = más lento y parabólico
        float flightTime = 1.4f; // Aumentado para hacerlo más visible y lento

        Vector3 toTarget = targetPos - spawnPos;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);
        float yOffset = toTarget.y;
        float gravity = Mathf.Abs(Physics.gravity.y);

        // Calcular velocidades
        float verticalVelocity = (yOffset + 0.5f * gravity * flightTime * flightTime) / flightTime;

        // Acentuar el arco parabólico ligeramente (opcional: +20%)
        verticalVelocity *= 1.2f;

        Vector3 horizontalVelocity = toTargetXZ / flightTime;
        Vector3 launchVelocity = horizontalVelocity + Vector3.up * verticalVelocity;

        // Aplicar velocidad
        rb.linearVelocity = launchVelocity;

        // Iniciar cuenta regresiva para explosión
        bomba.IniciarCuentaRegresiva();

        yield return new WaitForSeconds(4);
        isFiring = false;
    }


}
