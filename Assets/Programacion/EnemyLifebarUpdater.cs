using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLifebarUpdater : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private List<Renderer> lifebarQuads = new List<Renderer>();
    private int lastKnownHealth = -1;
    private Transform lifebar;
    private Coroutine hideCoroutine;

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
        lifebar = transform.Find("Lifebar");
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

            // Start hidden
            lifebar.gameObject.SetActive(false);
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
            ShowLifebarTemporarily();
            UpdateLifebar();
        }
    }

    private void ShowLifebarTemporarily()
    {
        if (lifebar != null)
        {
            lifebar.gameObject.SetActive(true);

            // Restart the timer if it's already running
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }
            hideCoroutine = StartCoroutine(HideLifebarAfterDelay(3f));
        }
    }

    private IEnumerator HideLifebarAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (lifebar != null)
        {
            lifebar.gameObject.SetActive(false);
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
