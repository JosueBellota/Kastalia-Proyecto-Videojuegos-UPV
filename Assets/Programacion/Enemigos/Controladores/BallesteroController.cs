using System.Collections;
using UnityEngine;

public class BallesteroController : Maquina
{
    public EnemyType tipoEnemigo = EnemyType.Ballestero;
    //Atributos relacionados al enemigo
    public float attackDamage = 20f;
    public float shootingDistance = 15f;
    public float safeDistance = 10f;
    private float arrowForce = 20f;
    public bool isFiring = false;

    //Atributos relacionados al jugador
    public Transform jugador;
    public float distanciaAJugador;
    
    //Estados
    public NombreEstado deambularEstado;
    public NombreEstado mantenerDistanciaEstado;
    public NombreEstado atacarEstado;
    public GameObject arrowPrefab;

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
            else{ jugador = FindFirstObjectByType<CharacterController>().transform; }
        }
        distanciaAJugador = getDistanceToPlayer();
    }

    private float getDistanceToPlayer(){
        if (jugador){
            return Vector3.Distance(transform.position, jugador.position);
        } else {
            Debug.LogError("No se ha encontrado al jugador.");
            return 0;
        }
    }

   public IEnumerator ShootArrow() 
    {
        isFiring = true;

        // 🔒 Locks in the player’s position
        Vector3 targetPosition = jugador.position;

        // ⏱ Waits BEFORE shooting (gives player time to dodge)
        float aimDelay = 1f;
        yield return new WaitForSeconds(aimDelay);

        // 📍 Calculates direction using saved position
        Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;
        Vector3 direction = (targetPosition - transform.position).normalized;

        // 🏹 Fires arrow toward saved position
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.LookRotation(direction));
        arrow.GetComponent<Arrow>().setDamage(attackDamage);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(direction * arrowForce, ForceMode.Impulse);

        // 🕒 Waits AGAIN after shooting to prevent spamming
        yield return new WaitForSeconds(aimDelay); // Optional reuse of aimDelay
        isFiring = false;
    }


}
