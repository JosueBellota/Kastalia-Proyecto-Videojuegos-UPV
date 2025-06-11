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
        // Weapon slot always remains active, just update the icon
        UpdateSlotIcon(WeaponButton.transform, weaponPrefab, weapon.icon);
    }
    public void updateHabilitySlots(AbilityType abilityType, Ability ability)
    {
        switch (abilityType)
        {
            case AbilityType.Ofensiva:
                UpdateSlotIcon(OffensiveButton.transform, ofensivaPrefab, ability.icon);
                break;
            case AbilityType.Defensiva:
                UpdateSlotIcon(ShieldButton.transform, defensivaPrefab, ability.icon);
                break;
            case AbilityType.Curativa:
                UpdateSlotIcon(PotionButton.transform, curativaPrefab, ability.icon);
                break;
        }
    }


    private void UpdateSlotIcon(Transform parent, GameObject prefab, Sprite icon)
    {
        // For weapon slot, we never destroy the prefab
        if (parent == WeaponButton.transform)
        {
            Image img = weaponPrefab.GetComponentInChildren<Image>(true);
            if (img != null && icon != null)
            {
                img.sprite = icon;
                img.color = Color.white;
            }
            return;
        }

        // For abilities - normal behavior
        Transform prefabInstance = parent.Find("AbilityPrefab");
        if (prefabInstance == null)
        {
            GameObject newPrefab = Instantiate(prefab, parent);
            newPrefab.name = "AbilityPrefab";
            prefabInstance = newPrefab.transform;
        }

        Image abilityImg = prefabInstance.GetComponentInChildren<Image>(true);
        if (abilityImg != null && icon != null)
        {
            abilityImg.sprite = icon;
            abilityImg.color = Color.white;
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
                Debug.Log("[UI] Icon was null — used default image or left as is.");
            }
        }
        else
        {
            Debug.LogWarning("[UI] No Image component found in the new prefab or its children.");
        }

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
        // First deactivate all selected indicators
        DeactivateAllSelectedIndicators();

        // Activate the appropriate selected indicator
        if (itemType == ItemType.Arma)
        {
            Transform selected = WeaponButton.transform.Find("selected");
            if (selected != null) selected.gameObject.SetActive(true);
            return;
        }

        switch (abilityType)
        {
            case AbilityType.Ofensiva:
                Transform offensiveSelected = OffensiveButton.transform.Find("selected");
                if (offensiveSelected != null) offensiveSelected.gameObject.SetActive(true);
                break;
            case AbilityType.Defensiva:
                Transform defensiveSelected = ShieldButton.transform.Find("selected");
                if (defensiveSelected != null) defensiveSelected.gameObject.SetActive(true);
                break;
            case AbilityType.Curativa:
                Transform curativeSelected = PotionButton.transform.Find("selected");
                if (curativeSelected != null) curativeSelected.gameObject.SetActive(true);
                break;
        }
    }

    private void DeactivateAllSelectedIndicators()
    {
        // Weapon
        Transform weaponSelected = WeaponButton.transform.Find("selected");
        if (weaponSelected != null) weaponSelected.gameObject.SetActive(false);

        // Offensive
        Transform offensiveSelected = OffensiveButton.transform.Find("selected");
        if (offensiveSelected != null) offensiveSelected.gameObject.SetActive(false);

        // Defensive
        Transform defensiveSelected = ShieldButton.transform.Find("selected");
        if (defensiveSelected != null) defensiveSelected.gameObject.SetActive(false);

        // Curative
        Transform curativeSelected = PotionButton.transform.Find("selected");
        if (curativeSelected != null) curativeSelected.gameObject.SetActive(false);
    }

    public void ClearAllAbilityPrefabs()
    {
        ClearAbilityPrefabFromButton(OffensiveButton.transform);
        ClearAbilityPrefabFromButton(ShieldButton.transform);
        ClearAbilityPrefabFromButton(PotionButton.transform);
    }

    public void ClearAbilityPrefab(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.Ofensiva:
                ClearAbilityPrefabFromButton(OffensiveButton.transform);
                break;
            case AbilityType.Defensiva:
                ClearAbilityPrefabFromButton(ShieldButton.transform);
                break;
            case AbilityType.Curativa:
                ClearAbilityPrefabFromButton(PotionButton.transform);
                break;
        }
    }

    private void ClearAbilityPrefabFromButton(Transform buttonTransform)
    {
        Transform abilityPrefab = buttonTransform.Find("AbilityPrefab");
        if (abilityPrefab != null)
        {
            Destroy(abilityPrefab.gameObject);
        }
    }

}