using System.Collections;
using UnityEngine;

public class HealingAbility : MonoBehaviour
{
    PlayerController playerController;
    PlayerInventory playerInventory;
    PlayerHealth playerHealth;
    public int healingAbilityCooldown = 0;


    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public IEnumerator healingAbility()
    {
        if (!playerInventory.equippedAbilities.TryGetValue(AbilityType.Curativa, out Ability healingAbility))
        {
            Debug.LogWarning("No offensive ability equipped!");
            yield break;
        }
        playerController.isCastingAbility = true;
        playerController.animator.SetTrigger("Healing");
        healingAbilityCooldown = healingAbility.killCountCooldown;
        playerHealth.healPlayer(60);
        playerController.isCastingAbility = false;
        if(playerInventory.selectedAbilityType == AbilityType.Curativa) {
            playerInventory.selectedItemType = ItemType.Arma;
        }
    }

}
