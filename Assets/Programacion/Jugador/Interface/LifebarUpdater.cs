using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarUpdater : MonoBehaviour
{
    private PlayerHealth playerHealth; // No longer public (assigned dynamically)
    private List<Image> lifebarSegments = new List<Image>();
    private float maxHealth;
    private float lastKnownHealth = -1f;

    void Start()
    {
        // Initialize UI segments
        foreach (Transform child in transform)
        {
            Image img = child.GetComponent<Image>();
            if (img != null) lifebarSegments.Add(img);
        }

        // Try to auto-detect player
        AssignPlayerHealth();
    }

    void Update()
    {
        if (playerHealth == null)
        {
            AssignPlayerHealth(); // Keep trying if player isn't spawned yet
            return;
        }

        if (playerHealth.vidaActual != lastKnownHealth)
        {
            lastKnownHealth = playerHealth.vidaActual;
            UpdateLifebar();
        }
    }

    // Dynamically find the active player's health
    private void AssignPlayerHealth()
    {
        if (LevelManager.instance != null && LevelManager.instance.player != null)
        {
            playerHealth = LevelManager.instance.player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                maxHealth = playerHealth.vidaMaxima;
                lastKnownHealth = playerHealth.vidaActual;
                UpdateLifebar();
            }
        }
    }

    private void UpdateLifebar()
    {
        float healthPercent = lastKnownHealth / maxHealth;
        int filledSegments = Mathf.CeilToInt(healthPercent * lifebarSegments.Count);

        for (int i = 0; i < lifebarSegments.Count; i++)
        {
            lifebarSegments[i].color = (i < filledSegments) ? Color.red : Color.white;
        }
    }
}