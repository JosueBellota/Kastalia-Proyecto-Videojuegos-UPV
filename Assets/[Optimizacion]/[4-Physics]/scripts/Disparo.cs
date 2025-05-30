using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    public Transform spawner;

    public GameObject proyectil;

    public float impulso = 300f;

    public void Disparar()
    {
        GameObject bala = Instantiate(proyectil, spawner.position, Quaternion.identity);
        bala.GetComponent<Rigidbody>().AddForce(spawner.up * impulso, ForceMode.VelocityChange);
    }
}
