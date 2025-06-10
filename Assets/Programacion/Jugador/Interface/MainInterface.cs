using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainInterface : MonoBehaviour
{
    [SerializeField] private TMP_Text VidaText;
    [SerializeField] private Button WeaponButton;
    [SerializeField] private Button OffensiveButton;
    [SerializeField] private Button ShieldButton;
    [SerializeField] private Button PotionButton;

    [Header("Prefabs")]
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject ofensivaPrefab;
    [SerializeField] private GameObject defensivaPrefab;
    [SerializeField] private GameObject curativaPrefab;

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
        ReplaceHabilidadImage(WeaponButton.transform, weaponPrefab, weapon.icon, weapon.name);
    }

    public void updateHabilitySlots(AbilityType abilityType, Ability ability)
    {
        switch (abilityType)
        {
            case AbilityType.Ofensiva:
                ReplaceHabilidadImage(OffensiveButton.transform, ofensivaPrefab, ability.icon, ability.name);
                break;
            case AbilityType.Defensiva:
                ReplaceHabilidadImage(ShieldButton.transform, defensivaPrefab, ability.icon, ability.name);
                break;
            case AbilityType.Curativa:
                ReplaceHabilidadImage(PotionButton.transform, curativaPrefab, ability.icon, ability.name);
                break;
        }
    }

    private void ReplaceHabilidadImage(Transform parent, GameObject prefab, Sprite icon, string fallbackName)
    {
        Debug.Log($"[UI] Updating slot: {parent.name} with ability {fallbackName}");

        Transform oldHabilidad = parent.Find("habilidad");
        if (oldHabilidad != null)
        {
            Debug.Log("[UI] Destroying old habilidad object.");
            Destroy(oldHabilidad.gameObject);
        }

        GameObject newHabilidad = Instantiate(prefab, parent);
        newHabilidad.name = "habilidad";
        
        // Try to find an Image component in the new object or its children
        Image img = newHabilidad.GetComponentInChildren<Image>(true);
        
        if (img != null)
        {
            if (icon != null)
            {
                img.sprite = icon;
                img.color = Color.white;
                Debug.Log("[UI] Sprite assigned successfully to new object.");
            }
            else
            {
                // Optional: assign a default placeholder sprite
                // img.sprite = defaultSprite; // You can define and serialize this
                Debug.Log("[UI] Icon was null — used default image or left as is.");
            }
        }
        else
        {
            Debug.LogWarning("[UI] No Image component found in the new prefab or its children.");
        }

        // Handle text fallback if no image is available at all
        if (icon == null || img == null)
        {
            TMP_Text text = newHabilidad.GetComponentInChildren<TMP_Text>(true);
            if (text != null) 
            {
                text.text = fallbackName;
                Debug.LogWarning("[UI] Using text fallback in new object.");
            }
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
            WeaponButton.image.color = Color.yellow;
            DarkenItems(ItemType.Arma, AbilityType.None);
            return;
        }

        if (abilityType == AbilityType.Ofensiva)
        {
            OffensiveButton.image.color = Color.yellow;
            DarkenItems(ItemType.Habilidad, AbilityType.Ofensiva);
            return;
        }
        if (abilityType == AbilityType.Defensiva)
        {
            ShieldButton.image.color = Color.yellow;
            DarkenItems(ItemType.Habilidad, AbilityType.Defensiva);
            return;
        }
        if (abilityType == AbilityType.Curativa)
        {
            PotionButton.image.color = Color.yellow;
            DarkenItems(ItemType.Habilidad, AbilityType.Curativa);
            return;
        }
    }

    public void SubtractCooldown()
    {
        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva))
        {
            if (offensiveAbilityController.offensiveAbilityCooldown == 0) { }
        }

        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva))
        {
            if (defensiveAbilityController.defensiveAbilityCooldown == 0) { }
        }

        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa))
        {
            if (healingAbilityController.healingAbilityCooldown == 0) { }
        }
    }

    private void DarkenItems(ItemType itemType, AbilityType abilityType)
    {
        if (itemType != ItemType.Arma) WeaponButton.image.color = Color.white;
        if (abilityType != AbilityType.Ofensiva) OffensiveButton.image.color = Color.white;
        if (abilityType != AbilityType.Defensiva) ShieldButton.image.color = Color.white;
        if (abilityType != AbilityType.Curativa) PotionButton.image.color = Color.white;
    }
}
