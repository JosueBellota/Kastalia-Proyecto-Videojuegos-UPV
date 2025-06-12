using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class WallProximityDetector : MonoBehaviour
{
    public float detectionRadius = 3f;
    public Material defaultMaterial;
    public Material proximityMaterial;

    private Renderer wallRenderer;
    
    private bool hasPlayedSound = false;

    private void Start()
    {



        SphereCollider sc = GetComponent<SphereCollider>();
        sc.isTrigger = true;
        sc.radius = detectionRadius;

        wallRenderer = GetComponent<Renderer>();

        if (wallRenderer == null)
        {
            // Debug.LogError("No se encontró un Renderer en el objeto.");
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

            if (localPos.z < 0f)
            {
                if (wallRenderer != null && proximityMaterial != null)
                {
                    wallRenderer.material = proximityMaterial;
                }
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 localPos = transform.InverseTransformPoint(other.transform.position);

            if (localPos.z < 0f)
            {
                if (wallRenderer != null && proximityMaterial != null && wallRenderer.material != proximityMaterial)
                {
                    wallRenderer.material = proximityMaterial;

                     // 🔊 Play Demon SFX when entering a "DemonWall"
                    if (gameObject.CompareTag("DemonWall") && !hasPlayedSound)
                    {
                        SFXManager.GetInstance()?.ReproducirDemon();
                        hasPlayedSound = true;
                    }
                }
            }
            else
            {
                if (wallRenderer != null && defaultMaterial != null && wallRenderer.material != defaultMaterial)
                {
                    wallRenderer.material = defaultMaterial;
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (wallRenderer != null && defaultMaterial != null)
            {
                wallRenderer.material = defaultMaterial;
            }
        }
    }
}
