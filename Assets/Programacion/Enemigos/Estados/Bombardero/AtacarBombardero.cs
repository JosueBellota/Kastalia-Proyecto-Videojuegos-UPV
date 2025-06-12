using UnityEngine;
using UnityEngine.AI;

public class AtacarBombardero : Estado
{
    NavMeshAgent agent;
    BombarderoController controller;
    float cooldown = 2f;
    float cooldownActual = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BombarderoController;
        controller.isFiring = false;
    }

    void Update()
    {
        if (!controller || !controller.jugador) return;
        if (controller.estaLanzandoBomba) return;

        transform.LookAt(controller.jugador);
        controller.velocidadActual = agent.velocity.magnitude;
        cooldownActual -= Time.deltaTime;

        if (controller.distanciaAJugador < controller.shootingDistance)
        {
            if (cooldownActual <= 0f)
            {
                controller.isFiring = true;
                controller.animator.SetTrigger("Atacar");
                cooldownActual = cooldown;
            }
        }
        else
        {
            if (controller.distanciaAJugador < controller.safeDistance)
            {
                if (controller.estaLanzandoBomba) return;
                agent.ResetPath();
                controller.SetEstado(controller.mantenerDistanciaEstado.Value);
            }
            else
            {
                if (controller.estaLanzandoBomba) return;
                agent.ResetPath();
                controller.SetEstado(controller.deambularEstado.Value);
            }
        }
    }
}
