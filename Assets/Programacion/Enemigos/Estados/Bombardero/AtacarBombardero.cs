using UnityEngine.AI;
using UnityEngine;

public class AtacarBombardero : Estado
{
    NavMeshAgent agent;
    BombarderoController controller;

    public float bombaForce = 8f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BombarderoController;
        controller.isFiring = false;
    }

    void Update()
    {
        transform.LookAt(controller.jugador);
        if (controller.jugador)
        {
            //Si el jugador se encuentra a tiro y lejos
            if (controller.distanciaAJugador < controller.shootingDistance && controller.distanciaAJugador > controller.safeDistance)
            {
                if (!controller.isFiring)
                {
                    controller.isFiring = true;
                    StartCoroutine(controller.ShootBomba());
                }
            }
            //Si el jugador se encuentra a demasiado cerca
            if (controller.distanciaAJugador < controller.safeDistance)
            {

                
                agent.ResetPath();
                transform.LookAt(controller.jugador);
                controller.SetEstado(controller.mantenerDistanciaEstado.Value);
            }
            if (controller.distanciaAJugador > controller.shootingDistance)
            {
                agent.ResetPath();
                controller.SetEstado(controller.deambularEstado.Value);
            }
        }
    }
}
