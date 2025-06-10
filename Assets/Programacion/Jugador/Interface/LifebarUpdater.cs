using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarUpdater : MonoBehaviour
{
    private PlayerHealth playerHealth; 
    private List<Image> lifebarSegments = new List<Image>();
    private float maxHealth;
    private float lastKnownHealth = -1f;

    [SerializeField] private Color emptyColor = new Color(1f, 1f, 1f, 0f);

    void Start()
    {
        foreach (Transform child in transform)
        {
            Image img = child.GetComponent<Image>();
            if (img != null) lifebarSegments.Add(img);
        }

        
        AssignPlayerHealth();
    }

    void Update()
    {
        if (playerHealth == null)
        {
            AssignPlayerHealth(); 
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
            lifebarSegments[i].color = (i < filledSegments) ? Color.red : emptyColor;
        }
    }
}