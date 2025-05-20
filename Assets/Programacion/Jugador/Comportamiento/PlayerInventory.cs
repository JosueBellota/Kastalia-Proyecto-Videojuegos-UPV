using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public WeaponType allowedWeaponType;
    public Weapon weapon;
    public ItemType selectedItemType;
    public AbilityType selectedAbilityType;
    public Dictionary<AbilityType, Ability> equippedAbilities = new Dictionary<AbilityType, Ability>();

    PlayerController controller;
    MainInterface hud;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        hud = FindFirstObjectByType<MainInterface>();
    }


    public void EquipWeapon(Weapon weapon)
    {
        if (weapon.weaponType == allowedWeaponType)
        {
            this.weapon = weapon;
            hud.updateWeaponSlot(weapon);
        }
    }

    public bool HasWeapon(Weapon other)
    {
        return weapon != null && weapon.name == other.name;
    }

    public void EquipAbility(Ability ability)
    {
        if (!equippedAbilities.ContainsKey(ability.abilityType))
        {
            equippedAbilities.Add(ability.abilityType, ability);
            hud.updateHabilitySlots(ability.abilityType, ability);
        }
    }

    public bool HasAbility(Ability ability)
    {
        return equippedAbilities.ContainsKey(ability.abilityType);
    }
}
