using UnityEngine;

public class Rotador : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public Vector3 velocidadRotacion = new Vector3(0, 100, 0);

    void Update()
    {
        // Rota el GameObject cada frame
        transform.Rotate(velocidadRotacion * Time.deltaTime);
    }
}
