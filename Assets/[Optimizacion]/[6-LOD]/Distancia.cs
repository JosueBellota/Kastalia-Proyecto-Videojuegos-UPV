using UnityEngine;
using UnityEngine.UI;

public class Distancia : MonoBehaviour
{
    [SerializeField] Transform Camara;
    [SerializeField] Transform Semaforo;

    Text text;
    Camera cam;

    private void Start()
    {
        text = GetComponent<Text>();
        cam = Camara.GetComponent<Camera>();
    }

    private void Update()
    {
        float distancia = Semaforo.position.z - Camara.position.z;
        string distStr = distancia.ToString("00.00");
        string farPlane = (distancia / cam.farClipPlane * 100).ToString("00.00");
        text.text = $"Distancia: {distStr} m / {farPlane} %";
    }

}
