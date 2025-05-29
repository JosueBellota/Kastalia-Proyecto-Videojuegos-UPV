using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{

    public float frecuenciaDisparo = 3f;

    public List<Transform> positions = new List<Transform>();

    public float smoothTime = 0.3f;

    public float stopOffset = 0.1f;

    Disparo disparo;

    Transform target;
    
    Vector3 velocity = Vector3.zero;

    void Start()
    {
        disparo = GetComponent<Disparo>();
        StartCoroutine(Disparar());
        GetNewTarget();
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        if(Vector3.Distance(transform.position, target.position) < stopOffset)
        {
            GetNewTarget();
        } 
    }

    IEnumerator Disparar()
    {
        while (true)
        {
            yield return new WaitForSeconds(frecuenciaDisparo);
            disparo.Disparar();
        }
    }

    void GetNewTarget()
    {
        int i = Random.Range(0, positions.Count);
        if (target != null) positions.Add(target);
        target = positions[i];
        positions.Remove(target);
    }
}
