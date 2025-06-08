using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AtacarBallestero : Estado
{
    NavMeshAgent agent;
    BallesteroController controller;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BallesteroController;
    }

    void Update()
    {
        if (!controller || !controller.jugador) return;

        transform.LookAt(controller.jugador);
        controller.velocidadActual = agent.velocity.magnitude;

        if (controller.distanciaAJugador < controller.shootingDistance && controller.distanciaAJugador > controller.safeDistance)
        {
            if (!controller.isFiring)
            {
                StartCoroutine(controller.ShootArrow()); // lanza flecha y activa animación
            }
        }
        else if (controller.distanciaAJugador < controller.safeDistance)
        {
            agent.ResetPath();
            transform.LookAt(controller.jugador);
            controller.SetEstado(controller.mantenerDistanciaEstado.Value);
        }
        else if (controller.distanciaAJugador > controller.shootingDistance)
        {
            agent.ResetPath();
            transform.LookAt(controller.jugador);
            controller.SetEstado(controller.deambularEstado.Value);
        }
    }
}
