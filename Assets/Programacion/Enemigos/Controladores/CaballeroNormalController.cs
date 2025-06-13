using System.Collections;
using UnityEngine;

public class CaballeroNormalController : Maquina
{
    public EnemyType tipoEnemigo = EnemyType.CaballeroNormal;
    Transform jugador;
    public float distanciaAtaque = 3f;
    [SerializeField] float distanciaDeteccion = 5f;

    public float attackDamage = 10f;

    public NombreEstado deambularEstado;
    public NombreEstado perseguirEstado;
    public NombreEstado atacarEstado;

    public float AttackDistance { get { return distanciaAtaque; } }
    public float DetectionDistance { get { return distanciaDeteccion; } }

    public Transform Player { get { return jugador; } }

    void Start()
    {
        if (GameObject.FindWithTag("Player") == null) return;
        jugador = GameObject.FindWithTag("Player").transform;

        if (deambularEstado != null)
        {
            SetEstado(deambularEstado.Value); // Activa estado deambular
        }

    }

    void Update()
    {
        if (GameObject.FindWithTag("Player") == null)
        {
            return;
        }
        else
        {
            jugador = GameObject.FindWithTag("Player").transform;
        }
    }
}

public enum EnemigoTipo { Melee, Distancia }
