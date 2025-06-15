using System.Collections.Generic;
using UnityEngine;

public class EnemyLifebarUpdater : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private List<Renderer> lifebarQuads = new List<Renderer>();
    private int lastKnownHealth = -1;

    void Start()
    {
        // Automatically get EnemyHealth from the same GameObject
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            Debug.LogError("[EnemyLifebar] EnemyHealth not found on the same GameObject.");
            return;
        }

        // Find the Lifebar child and collect its Quads
        Transform lifebar = transform.Find("Lifebar");
        if (lifebar != null)
        {
            foreach (Transform child in lifebar)
            {
                Renderer rend = child.GetComponent<Renderer>();
                if (rend != null)
                {
                    lifebarQuads.Add(rend);
                }
            }
        }
        else
        {
            Debug.LogError("[EnemyLifebar] 'Lifebar' child not found under this GameObject.");
        }

        lastKnownHealth = enemyHealth.maxHealth;
        UpdateLifebar();
    }

    void Update()
    {
        if (enemyHealth == null) return;

        int currentHealth = enemyHealth.CurrentHealth();
        if (enemyHealth.maxHealth != 0 && currentHealth != lastKnownHealth)
        {
            Debug.Log($"[EnemyLifebar] Enemy took damage: {lastKnownHealth} → {currentHealth}");
            lastKnownHealth = currentHealth;
            UpdateLifebar();
        }
    }

    private void UpdateLifebar()
    {
        int segments = lifebarQuads.Count;
        if (segments == 0) return;

        float healthPerSegment = (float)enemyHealth.maxHealth / segments;
        int activeSegments = Mathf.CeilToInt(enemyHealth.CurrentHealth() / healthPerSegment);

        for (int i = 0; i < lifebarQuads.Count; i++)
        {
            bool shouldBeActive = i < activeSegments;

            if (lifebarQuads[i].gameObject.activeSelf != shouldBeActive)
            {
                lifebarQuads[i].gameObject.SetActive(shouldBeActive);
                Debug.Log($"[EnemyLifebar] Quad {i + 1} {(shouldBeActive ? "enabled" : "disabled")}.");
            }
        }
    }
}
