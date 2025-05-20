using System.Collections;
using UnityEngine;

public class DefensiveAbility : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerInventory playerInventory;
    private PlayerHealth playerHealth;
    public int defensiveAbilityCooldown = 0;


    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void enableShield()
    {
        if (!playerInventory.equippedAbilities.TryGetValue(AbilityType.Defensiva, out Ability defensiveAbility))
        {
            Debug.LogWarning("No defensive ability equipped!");
            return;
        }
        playerController.isCastingAbility = true;
        playerController.animator.SetTrigger("Shield");
        playerHealth.defensiveAbilityHits = 2;
        playerController.ToggleShieldPrefab(true);
        Debug.Log("Shield enabled");
        defensiveAbilityCooldown = defensiveAbility.killCountCooldown;
        playerController.isCastingAbility = false;
        StartCoroutine(shieldDuration());
        if (playerInventory.selectedAbilityType == AbilityType.Defensiva)
        {
            playerInventory.selectedItemType = ItemType.Arma;
        }
    }

    public IEnumerator shieldDuration()
    {
        yield return new WaitForSeconds(10);
        playerHealth.defensiveAbilityHits = 0;
        playerController.ToggleShieldPrefab(false);
        Debug.Log("Shield disabled");

        playerInventory.selectedItemType = ItemType.Arma;
    }
}
