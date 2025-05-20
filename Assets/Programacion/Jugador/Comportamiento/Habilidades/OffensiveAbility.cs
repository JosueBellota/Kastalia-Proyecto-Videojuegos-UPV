using System.Collections;
using UnityEngine;

public class OffensiveAbility : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private PlayerController playerController;
    private PosicionCursor posicionCursor;
    private MainInterface mainInterface;
    private Animator animator;
    public int offensiveAbilityCooldown = 0;

    [SerializeField] GameObject abilityTargetIndicatorPrefab;
    [SerializeField] private GameObject fireballPrefab;
    private float fireballSpeed = 25f;
    private GameObject currentIndicator;


    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerController = GetComponent<PlayerController>();
        posicionCursor = GetComponent<PosicionCursor>();
        mainInterface = FindFirstObjectByType<MainInterface>();

        animator = playerController.animator;
    }
    public IEnumerator offensiveAbility()
    {
        if (!playerInventory.equippedAbilities.TryGetValue(AbilityType.Ofensiva, out Ability offensiveAbility))
        {
            Debug.LogWarning("No offensive ability equipped!");
            yield break;
        }

        float maxCastRange = playerInventory.equippedAbilities[AbilityType.Ofensiva].range;
        float area = playerInventory.equippedAbilities[AbilityType.Ofensiva].areaOfEffect;

        // Spawn the indicator
        if (abilityTargetIndicatorPrefab != null)
        {
            currentIndicator = Instantiate(abilityTargetIndicatorPrefab);
            currentIndicator.transform.localScale = new Vector3(area, 0.1f, area);
        }

        // Aiming loop
        while (!Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 targetPosition = GetClampedCursorPosition(maxCastRange);
            if (currentIndicator != null)
            {
                currentIndicator.transform.position = targetPosition;
            }

            yield return null; // wait for next frame
        }

        // Confirmed cast
        Vector3 finalTargetPosition = currentIndicator.transform.position;
        Destroy(currentIndicator);

        yield return new WaitForSeconds(0.2f); // small casting delay

        FireProjectile(posicionCursor.lookPoint);

        offensiveAbilityCooldown = offensiveAbility.killCountCooldown;
        mainInterface.LightUpItem(ItemType.Arma, AbilityType.None);
        playerInventory.selectedItemType = ItemType.Arma;
        playerController.ShowWeapon(true);
    }
    private Vector3 GetClampedCursorPosition(float maxRange)
    {
        Vector3 targetPosition = posicionCursor.lookPoint;
        Vector3 playerPosition = transform.position;

        float distance = Vector3.Distance(playerPosition, targetPosition);
        if (distance > maxRange)
        {
            Vector3 direction = (targetPosition - playerPosition).normalized;
            targetPosition = playerPosition + direction * maxRange;
        }

        return targetPosition;
    }
    private void FireProjectile(Vector3 targetPosition)
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position + Vector3.up * 5f + Vector3.forward * 5, Quaternion.identity);
        animator.SetTrigger("Fireball");

        Vector3 direction = (targetPosition - fireball.transform.position).normalized;

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        rb.AddForce(direction * fireballSpeed, ForceMode.VelocityChange);
        rb.useGravity = true;

        Fireball fbScript = fireball.GetComponent<Fireball>();
        if (fbScript != null)
        {
            fbScript.SetDamage(playerInventory.equippedAbilities[AbilityType.Ofensiva].damage);
            fbScript.SetAreaOfEffect(playerInventory.equippedAbilities[AbilityType.Ofensiva].areaOfEffect);
        }

    }

}

