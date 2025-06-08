using UnityEngine.AI;
using UnityEngine;

public class AtacarBombardero : Estado
{
    NavMeshAgent agent;
    BombarderoController controller;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BombarderoController;
        controller.isFiring = false;
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
        controller.isFiring = true;
        controller.animator.SetTrigger("Atacar"); // ← lanza la animación directamente
        StartCoroutine(controller.ShootBomba());
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
            controller.SetEstado(controller.deambularEstado.Value);
        }
    }
}
