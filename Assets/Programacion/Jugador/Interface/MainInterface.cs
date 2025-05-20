using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainInterface : MonoBehaviour
{
    [SerializeField] private TMP_Text VidaText;
    [SerializeField] private Button WeaponButton;
    [SerializeField] private Button OffensiveButton;
    [SerializeField] private Button ShieldButton;
    [SerializeField] private Button PotionButton;

    private bool isPlayerFound = false;
    private GameObject player;
    private PlayerInventory playerInventory;
    private OffensiveAbility offensiveAbilityController;
    private DefensiveAbility defensiveAbilityController;
    private HealingAbility healingAbilityController;


    public void updateVidaText(float vida)
    {
        VidaText.text = vida.ToString() + " / 100";
    }

    public void updateWeaponSlot(Weapon weapon)
    {
        if (weapon.icon)
        {
            WeaponButton.GetComponent<Image>().sprite = weapon.icon;
            return;
        }
        WeaponButton.GetComponentInChildren<TMP_Text>().text = weapon.name;
    }

    public void updateHabilitySlots(AbilityType abilityType, Ability ability)
    {
        switch (abilityType)
        {
            case AbilityType.Ofensiva:
                Debug.Log(ability.abilityName);
                OffensiveButton.GetComponentInChildren<TMP_Text>().text = ability.abilityName;
                break;
            case AbilityType.Defensiva:
                ShieldButton.GetComponentInChildren<TMP_Text>().text = ability.abilityName;
                break;
            case AbilityType.Curativa:
                PotionButton.GetComponentInChildren<TMP_Text>().text = ability.abilityName;
                break;
        }
    }

    void Update()
    {
        if (!isPlayerFound)
        {
            if (!player)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                return;
            }
            if (FindPlayerAtributes())
            {
                isPlayerFound = true;
            }
        }
    }


    private bool FindPlayerAtributes()
    {
        if (player)
        {
            if (player.GetComponent<OffensiveAbility>() &&
            player.GetComponent<DefensiveAbility>() &&
            player.GetComponent<HealingAbility>() &&
            player.GetComponent<PlayerInventory>())
            {
                offensiveAbilityController = player.GetComponent<OffensiveAbility>();
                defensiveAbilityController = player.GetComponent<DefensiveAbility>();
                healingAbilityController = player.GetComponent<HealingAbility>();
                playerInventory = player.GetComponent<PlayerInventory>();

                return true;
            }
        }
        return false;
    }

    public void LightUpItem(ItemType itemType, AbilityType abilityType)
    {
        if (itemType == ItemType.Arma)
        {
            WeaponButton.GetComponent<Image>().color = Color.yellow;
            DarkenItems(ItemType.Arma, AbilityType.None);
            return;
        }

        if (abilityType == AbilityType.Ofensiva)
        {
            OffensiveButton.GetComponent<Image>().color = Color.yellow;
            DarkenItems(ItemType.Habilidad, AbilityType.Ofensiva);
            return;
        }
        if (abilityType == AbilityType.Defensiva)
        {
            ShieldButton.GetComponent<Image>().color = Color.yellow;
            DarkenItems(ItemType.Habilidad, AbilityType.Defensiva);
            return;
        }
        if (abilityType == AbilityType.Curativa)
        {
            PotionButton.GetComponent<Image>().color = Color.yellow;
            DarkenItems(ItemType.Habilidad, AbilityType.Curativa);
            return;
        }
    }

    public void SubtractCooldown()
    {
        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva))
        {
            if (offensiveAbilityController.offensiveAbilityCooldown == 0)
            {
                OffensiveButton.GetComponentInChildren<TMP_Text>().text = playerInventory.equippedAbilities[AbilityType.Ofensiva].abilityName;
            }
            else
            {
                OffensiveButton.GetComponentInChildren<TMP_Text>().text = offensiveAbilityController.offensiveAbilityCooldown.ToString("F1");
            }
        }

        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva))
        {
            if (defensiveAbilityController.defensiveAbilityCooldown == 0)
            {
                ShieldButton.GetComponentInChildren<TMP_Text>().text = playerInventory.equippedAbilities[AbilityType.Defensiva].abilityName;
            }
            else
            {
                ShieldButton.GetComponentInChildren<TMP_Text>().text = defensiveAbilityController.defensiveAbilityCooldown.ToString("F1");
            }
        }

        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa))
        {
            if (healingAbilityController.healingAbilityCooldown == 0)
            {
                PotionButton.GetComponentInChildren<TMP_Text>().text = playerInventory.equippedAbilities[AbilityType.Curativa].abilityName;
            }
            else
            {
                PotionButton.GetComponentInChildren<TMP_Text>().text = healingAbilityController.healingAbilityCooldown.ToString("F1");
            }
        }
    }

    private void DarkenItems(ItemType itemType, AbilityType abilityType)
    {
        if (itemType != ItemType.Arma) WeaponButton.GetComponent<Image>().color = Color.white;
        if (abilityType != AbilityType.Ofensiva) OffensiveButton.GetComponent<Image>().color = Color.white;
        if (abilityType != AbilityType.Defensiva) ShieldButton.GetComponent<Image>().color = Color.white;
        if (abilityType != AbilityType.Curativa) PotionButton.GetComponent<Image>().color = Color.white;
    }
}
