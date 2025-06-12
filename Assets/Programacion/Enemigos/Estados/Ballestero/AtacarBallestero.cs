using UnityEngine;
using UnityEngine.AI;

public class AtacarBallestero : Estado
{
    NavMeshAgent agent;
    BallesteroController controller;

    float cooldown = 1.5f;
    float cooldownActual = 0f;

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
        cooldownActual -= Time.deltaTime;

        if (controller.distanciaAJugador < controller.shootingDistance && controller.distanciaAJugador > controller.safeDistance)
        {
            if (cooldownActual <= 0f && !controller.estaDisparando)
            {
                controller.isFiring = true;
                controller.animator.SetTrigger("Atacar"); // ← evento llama a LanzarFlecha
                cooldownActual = cooldown;
            }
        }
        else if (controller.distanciaAJugador <= controller.safeDistance)
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
