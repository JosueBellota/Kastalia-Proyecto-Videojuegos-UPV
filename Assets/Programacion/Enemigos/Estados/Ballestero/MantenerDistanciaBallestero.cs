using UnityEngine;
using UnityEngine.AI;

public class MantenerDistanciaBallestero : Estado
{
    NavMeshAgent agent;
    Transform player;
    BallesteroController controller;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BallesteroController;
        player = controller.jugador;
    }

    void Update()
    {
        if (GameManager.instance.isPaused) return;

        if (controller != null && controller.jugador != null)
        {
            float distancia = controller.distanciaAJugador;
            Vector3 directionToPlayer = (controller.jugador.position - transform.position).normalized;

            if (distancia < controller.safeDistance)
            {
                transform.LookAt(controller.jugador);
                if (!controller.isFiring)
                {
                    StartCoroutine(controller.ShootArrow());
                }
                Vector3 fleePosition = transform.position - directionToPlayer * 2f;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(fleePosition, out hit, 2f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
            else if (distancia <= controller.shootingDistance)
            {
                agent.ResetPath();
                transform.LookAt(controller.jugador);
                controller.SetEstado(controller.atacarEstado.Value);
            }
            else
            {
                agent.SetDestination(controller.jugador.position);
            }
        }
    }

}

