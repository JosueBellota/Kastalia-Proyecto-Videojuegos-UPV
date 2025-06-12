using UnityEngine;
using UnityEngine.AI;

public class MantenerDistanciaBallestero : Estado
{
    NavMeshAgent agent;
    BallesteroController controller;
    float cooldown = 1.5f;
    float tiempoRestante = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BallesteroController;
    }

    void Update()
    {
        if (GameManager.instance.isPaused || controller == null || controller.jugador == null) return;

        float distancia = controller.distanciaAJugador;
        Vector3 directionToPlayer = (controller.jugador.position - transform.position).normalized;

        controller.velocidadActual = agent.velocity.magnitude;
        tiempoRestante -= Time.deltaTime;

        if (distancia < controller.safeDistance)
        {
            transform.LookAt(controller.jugador);

            // Disparo si no estoy disparando ni lanzando, y ha pasado el cooldown
            if (!controller.isFiring && !controller.estaDisparando && tiempoRestante <= 0f)
            {
                controller.isFiring = true;
                controller.animator.SetTrigger("Atacar");
                tiempoRestante = cooldown;
            }

            // Huir un poco hacia atrás
            Vector3 fleePosition = transform.position - directionToPlayer * 2f;
            if (NavMesh.SamplePosition(fleePosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        else if (distancia <= controller.shootingDistance)
        {
            agent.ResetPath();
            controller.SetEstado(controller.atacarEstado.Value);
        }
        else
        {
            agent.SetDestination(controller.jugador.position);
        }
    }
}