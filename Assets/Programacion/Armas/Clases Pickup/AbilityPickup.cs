using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public Ability abilityData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventario = other.GetComponent<PlayerInventory>();

        if (inventario && !inventario.HasAbility(abilityData))
        {
            inventario.EquipAbility(abilityData);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Ya tienes esta habilidad equipada.");
        }
    }
}
