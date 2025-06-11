using System.Collections;
using UnityEngine;

public class HealingAbility : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerInventory playerInventory;
    private PlayerHealth playerHealth;

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
            Debug.LogWarning("No healing ability equipped!");
            yield break;
        }

        playerController.isCastingAbility = true;
        playerController.animator.SetTrigger("Healing");
        
       
        playerHealth.healPlayer(60); 
        

        yield return new WaitForSeconds(0.5f); 
        
        playerController.isCastingAbility = false;

    }
}