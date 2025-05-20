using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EnemyHighlighter : MonoBehaviour
{
    [Header("Highlight Settings")]
    [SerializeField] private Color highlightColor = Color.red;
    [SerializeField] private float outlineWidth = 0.1f;

    private Material originalMaterial;
    private Material outlineMaterial;
    private Renderer enemyRenderer;

    private bool isMouseOver = false;
    private bool isLocked = false;

    private void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
        originalMaterial = enemyRenderer.material;

        // Crear material de contorno
        outlineMaterial = new Material(Shader.Find("Custom/OutlineShader"));
        outlineMaterial.SetColor("_Color", originalMaterial.color);
        outlineMaterial.SetTexture("_MainTex", originalMaterial.mainTexture);
        outlineMaterial.SetColor("_OutlineColor", highlightColor);
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
    }

    private void Update()
    {
        UpdateOutline();
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

    private void UpdateOutline()
    {
        Material[] currentMaterials = enemyRenderer.materials;

        bool shouldShowOutline = isMouseOver || isLocked;

        if (shouldShowOutline && currentMaterials.Length == 1)
        {
            Material[] newMaterials = new Material[2];
            newMaterials[0] = originalMaterial;
            newMaterials[1] = outlineMaterial;
            enemyRenderer.materials = newMaterials;
        }
        else if (!shouldShowOutline && currentMaterials.Length == 2)
        {
            Material[] newMaterials = new Material[1];
            newMaterials[0] = originalMaterial;
            enemyRenderer.materials = newMaterials;
        }
    }

    private void OnDestroy()
    {
        if (outlineMaterial != null)
        {
            Destroy(outlineMaterial);
        }
    }
}
