using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DeambularBombardero : Estado
{
    private float radio = 5f;
    private Vector3 posicionAleatoria;
    NavMeshAgent agent;
    BombarderoController controller;
    private bool estaDeambulando = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BombarderoController;
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
        Vector3 randomDirection = Random.insideUnitSphere * radio;
        randomDirection.y = 0;
        Vector3 targetPosition = agent.transform.position + randomDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, radio, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return agent.transform.position;
        }
    }


    IEnumerator DeambularCoorutina()
    {
        if (agent == null) yield break;
        if (!estaDeambulando)
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
}
