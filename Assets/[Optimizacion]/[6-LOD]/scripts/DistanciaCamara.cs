using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanciaCamara : MonoBehaviour
{

    public float velocidad = 3f;

    Vector3 min;

    void Start()
    {
        min = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * velocidad);
        if(transform.position.z > min.z)
        {
            transform.position = min;
        }
    }
}
