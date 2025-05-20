using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Atacar : Estado
{
    CaballeroNormalController controller;
    Transform player;
    NavMeshAgent agent;
    bool puedeAtacar = true;

    public float damage = 20f;
    public EnemigoTipo tipoDeEnemigo;

    [SerializeField] float distanceToPlayer;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as CaballeroNormalController;
        agent.ResetPath();
    }

    private void Update()
    {
        if (player == null) return;
        DistanceToPlayer();

        // Check if within visual attack range and can attack
        if (distanceToPlayer <= controller.distanciaAtaque && puedeAtacar )
        {
            StartCoroutine(LoopAtaque());
        }

        if (distanceToPlayer > controller.AttackDistance)
        {
            puedeAtacar = true;
            StopAllCoroutines();
            maquina.SetEstado(controller.perseguirEstado.Value);
        }
    }


    IEnumerator LoopAtaque()
    {
        puedeAtacar = false;
        if (player != null)
        {
            PlayerHealth PlayerHealth = player.GetComponent<PlayerHealth>();
            if (PlayerHealth != null)
            {
                PlayerHealth.takeDamage(controller.attackDamage);
            }

            yield return new WaitForSeconds(1f); // Cooldown
        }
        puedeAtacar = true;
    }

    private void DistanceToPlayer()
    {
        if (player == null) return;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }
}
