using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventario = other.GetComponent<PlayerInventory>();

        if (inventario && !inventario.HasWeapon(weaponData))
        {
            inventario.EquipWeapon(weaponData);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Ya tienes esta arma equipada.");
        }
    }
}
