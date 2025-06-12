using UnityEngine;
using UnityEngine.AI;

public class MantenerDistanciaBombardero : Estado
{
    NavMeshAgent agent;
    BombarderoController controller;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BombarderoController;
    }

    void Update()
    {
        if (GameManager.instance.isPaused || controller == null || controller.jugador == null) return;
        if (controller.estaLanzandoBomba) return;

        float distancia = controller.distanciaAJugador;
        Vector3 directionToPlayer = (controller.jugador.position - transform.position).normalized;

        if (distancia < controller.safeDistance)
        {
            transform.LookAt(controller.jugador);

            if (!controller.isFiring)
            {
                controller.isFiring = true;
                controller.animator.SetTrigger("Atacar");
            }

            Vector3 fleePosition = transform.position - directionToPlayer * 2f;
            if (NavMesh.SamplePosition(fleePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                agent.SetDestination(hit.position);
        }
        else if (distancia <= controller.shootingDistance)
        {
            if (controller.estaLanzandoBomba) return;
            agent.ResetPath();
            controller.SetEstado(controller.atacarEstado.Value);
        }
        else
        {
            agent.SetDestination(controller.jugador.position);
        }
    }
}