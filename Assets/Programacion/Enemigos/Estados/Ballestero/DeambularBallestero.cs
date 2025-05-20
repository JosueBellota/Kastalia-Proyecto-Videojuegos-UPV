using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DeambularEstadoBallestero : Estado
{
    private float radio = 5f;
    private Vector3 posicionAleatoria;
    NavMeshAgent agent;
    BallesteroController controller;
    private bool estaDeambulando = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BallesteroController;
    }

    void Update()
    {
        if (GameManager.instance.isPaused) return;
        if (controller != null && controller.jugador != null)
        {
            if (controller.distanciaAJugador <= controller.shootingDistance && controller.distanciaAJugador > controller.safeDistance)
            {
                StopCoroutine(DeambularCoorutina());
                estaDeambulando = false;
                transform.LookAt(controller.jugador);
                controller.DisplayAgroPopup();
                controller.SetEstado(controller.atacarEstado.Value);
            }
            else if (controller.distanciaAJugador <= controller.safeDistance)
            {
                StopCoroutine(DeambularCoorutina());
                estaDeambulando = false;
                transform.LookAt(controller.jugador);
                controller.DisplayAgroPopup();
                controller.SetEstado(controller.mantenerDistanciaEstado.Value);
            }
            else
            {
                if (!estaDeambulando)
                {
                    StartCoroutine(DeambularCoorutina());
                }
            }
        }
    }

    private Vector3 ElegirPosicionAleatoria()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 direccion2D = Random.insideUnitCircle * radio;
            Vector3 punto = transform.position + new Vector3(direccion2D.x, 0, direccion2D.y);

            if (NavMesh.SamplePosition(punto, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }


    IEnumerator DeambularCoorutina()
    {
        estaDeambulando = true;
        posicionAleatoria = ElegirPosicionAleatoria();

        while (posicionAleatoria != null && Vector3.Distance(agent.transform.position, posicionAleatoria) > .5f)
        {

            agent.SetDestination(posicionAleatoria);
            agent.transform.LookAt(posicionAleatoria);

            while (agent.pathPending || (agent.isOnNavMesh && agent.remainingDistance > .2f))
            {
                yield return null;
            }

            float tiempoEspera = Random.Range(1f, 4f);
            yield return new WaitForSeconds(tiempoEspera);
        }

        estaDeambulando = false;
    }
}
