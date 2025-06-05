using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.2f;

    private List<Renderer> renderers = new List<Renderer>();
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

    void Awake()
    {
        // Find all renderers in children (this includes SkinnedMeshRenderer and MeshRenderer)
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        // Store their original materials
        foreach (var rend in renderers)
        {
            originalMaterials[rend] = rend.materials;
        }
    }

    public void Flash()
    {
        Debug.Log("Player took damage!");

        if (flashMaterial == null)
        {
            Debug.LogWarning("No flash material assigned.");
            return;
        }

        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Replace each renderer's materials with flash material
        foreach (var rend in renderers)
        {
            Material[] flashMats = new Material[rend.materials.Length];
            for (int i = 0; i < flashMats.Length; i++)
            {
                flashMats[i] = flashMaterial;
            }
            rend.materials = flashMats;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore original materials
        foreach (var rend in renderers)
        {
            if (originalMaterials.ContainsKey(rend))
            {
                rend.materials = originalMaterials[rend];
            }
        }
    }
}
