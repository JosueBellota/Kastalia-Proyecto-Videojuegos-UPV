using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DeambularCaballeroNormal : Estado
{
    private float radio = 30f;
    private Vector3 posicionAleatoria;
    private NavMeshAgent agent;
    private CaballeroNormalController controller;
    private bool estaDeambulando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as CaballeroNormalController;
    }
    
    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isPaused) return;
        if (controller != null && controller.Player != null)
        {
            float distanciaAJugador = Vector3.Distance(transform.position, controller.Player.position);
            if (distanciaAJugador <= controller.DetectionDistance)
            {
                StopAllCoroutines();
                estaDeambulando = false;
                controller.DisplayAgroPopup();
                maquina.SetEstado(controller.perseguirEstado.Value);
            } else if(!estaDeambulando){
                estaDeambulando = true;
                StartCoroutine(DeambularCoorutina());
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
        while (true)
        {
            // Espera antes de elegir un punto (mirar)
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            posicionAleatoria = ElegirPosicionAleatoria();
            agent.SetDestination(posicionAleatoria);
            transform.LookAt(posicionAleatoria);

            while (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                yield return null;
            }

          
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }
}