using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class WallProximityDetector : MonoBehaviour
{
    public float detectionRadius = 3f;
    public Material defaultMaterial;
    public Material proximityMaterial;

    private Renderer wallRenderer;

    private void Start()
    {
        SphereCollider sc = GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = detectionRadius;

        wallRenderer = GetComponent<Renderer>();

        if (wallRenderer == null)
        {
            Debug.LogError("No se encontró un Renderer en el objeto.");
        }
        else
        {
            wallRenderer.material = defaultMaterial; // Asegura que empiece con el material por defecto
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("jugador cerca");
            if (wallRenderer != null && proximityMaterial != null)
            {
                wallRenderer.material = proximityMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("jugador se alejó");
            if (wallRenderer != null && defaultMaterial != null)
            {
                wallRenderer.material = defaultMaterial;
            }
        }
    }
}
