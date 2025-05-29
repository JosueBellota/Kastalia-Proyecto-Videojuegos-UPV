using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField] float speed = 30f;
    
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
