using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannon : MonoBehaviour
{

    public float velocidad = 5f;

    public Transform min;

    public Transform max;

    Disparo disparo;

    void Start()
    {
        disparo = GetComponent<Disparo>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            disparo.Disparar();
        }

        float move = velocidad * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.Translate(Vector3.right * move);

        if(transform.position.x > max.position.x)
        {
            transform.position = max.position;
        }

        if(transform.position.x < min.position.x)
        {
            transform.position = min.position;
        }
    }
}
