using System.Collections;
using UnityEngine;

public class BallesteroController : Maquina
{
    public EnemyType tipoEnemigo = EnemyType.Ballestero;
    //Atributos relacionados al enemigo
    public float attackDamage = 20f;
    public float shootingDistance = 15f;
    public float safeDistance = 10f;
    public float fireCooldown = 1f;
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
        Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;
        Vector3 direction = (jugador.position - transform.position).normalized;

        GameObject arrow = Instantiate(arrowPrefab, spawnPos, Quaternion.LookRotation(direction));
        arrow.GetComponent<Arrow>().setDamage(attackDamage);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(direction * arrowForce, ForceMode.Impulse);

        yield return new WaitForSeconds(fireCooldown);
        isFiring = false;
    }
}
