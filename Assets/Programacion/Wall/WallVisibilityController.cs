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
            Vector3 localPos = transform.InverseTransformPoint(other.transform.position);
            Debug.Log($"Jugador entró - Z local: {localPos.z:F2}");

            if (localPos.z < 0f)
            {
                Debug.Log("jugador en frente, cambiando material");
                if (wallRenderer != null && proximityMaterial != null)
                {
                    wallRenderer.material = proximityMaterial;
                }
            }
            else
            {
                Debug.Log("jugador detrás, no se cambia el material");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 localPos = transform.InverseTransformPoint(other.transform.position);
            Debug.Log($"Jugador dentro - Z local: {localPos.z:F2}");

            if (localPos.z < 0f)
            {
                if (wallRenderer != null && proximityMaterial != null && wallRenderer.material != proximityMaterial)
                {
                    wallRenderer.material = proximityMaterial;
                    Debug.Log("Jugador en frente dentro del trigger, cambiando a material de proximidad");
                }
            }
            else
            {
                if (wallRenderer != null && defaultMaterial != null && wallRenderer.material != defaultMaterial)
                {
                    wallRenderer.material = defaultMaterial;
                    Debug.Log("Jugador detrás dentro del trigger, restaurando material por defecto");
                }
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
