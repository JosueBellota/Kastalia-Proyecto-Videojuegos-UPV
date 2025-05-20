using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Perseguir : Estado
{
    NavMeshAgent agent;
    CaballeroNormalController controller;

    bool esperando = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as CaballeroNormalController;
        controller = GetComponent<CaballeroNormalController>();
    }

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.isPaused) return;
        if (controller == null || controller.Player == null) return;

        float distancia = Vector3.Distance(transform.position, controller.Player.position);

        if (distancia <= controller.AttackDistance)
        {
            controller.SetEstado(controller.atacarEstado.Value);
            return;
        }

        if (distancia <= controller.DetectionDistance)
        {
            agent.SetDestination(controller.Player.position);

            Vector3 direccion = controller.Player.position - transform.position;
            direccion.y = 0;
            if (direccion != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direccion);


        }
        else if (!esperando)
        {
            StartCoroutine(EsperarYVolverADeambular());
        }
    }

    IEnumerator EsperarYVolverADeambular()
    {
        esperando = true;
        agent.ResetPath();

        // Se queda mirando hacia la última posicion del jugador
        if (controller.Player != null)
        {
            Vector3 direccion = new Vector3(controller.Player.position.x, transform.position.y, controller.Player.position.z);
            transform.LookAt(direccion);
        }

        yield return new WaitForSeconds(2f);

        controller.SetEstado(controller.deambularEstado.Value);
    }
}