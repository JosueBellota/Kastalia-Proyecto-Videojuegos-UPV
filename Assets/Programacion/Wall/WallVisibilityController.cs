using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WallVisibilityController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fadeDistance = 3f;
    [SerializeField] private Material transparentMaterial;

    private Renderer wallRenderer;
    private Material[] originalMaterials;
    private Transform playerTransform;
    private bool isTransparent = false;

    private void Start()
    {
        wallRenderer = GetComponent<Renderer>();
        originalMaterials = wallRenderer.sharedMaterials;

        Debug.Log("[WallVisibility] Initialized on object: " + gameObject.name);
    }

    private void Update()
    {
        if (playerTransform == null && GameManager.instance != null && GameManager.instance.personajeSeleccionado != null)
        {
            playerTransform = GameManager.instance.personajeSeleccionado.transform;
            Debug.Log("[WallVisibility] Player found: " + playerTransform.name);
        }

        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        bool shouldBeTransparent = distance < fadeDistance;

        if (shouldBeTransparent != isTransparent)
        {
            if (shouldBeTransparent)
            {
                // Replace all materials with the transparent one
                Material[] transparentMats = new Material[wallRenderer.sharedMaterials.Length];
                for (int i = 0; i < transparentMats.Length; i++)
                {
                    transparentMats[i] = transparentMaterial;
                }
                wallRenderer.sharedMaterials = transparentMats;
            }
            else
            {
                // Restore original materials
                wallRenderer.sharedMaterials = originalMaterials;
            }

            isTransparent = shouldBeTransparent;
            Debug.Log($"[WallVisibility] Wall '{gameObject.name}' material switched to {(isTransparent ? "TRANSPARENT" : "ORIGINAL")} at distance {distance:F2}");
        }
    }
}
