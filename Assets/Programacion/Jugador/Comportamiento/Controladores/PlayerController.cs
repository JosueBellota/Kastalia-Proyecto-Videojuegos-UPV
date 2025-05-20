using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PlayerController : MonoBehaviour
{
    //Variables relacionadas al movimiento
    protected CharacterController controller;
    protected PlayerInventory playerInventory;
    protected PlayerHealth playerHealth;
    protected OffensiveAbility offensiveAbilityController;
    protected DefensiveAbility defensiveAbilityController;
    protected HealingAbility healingAbilityController;

    protected MainInterface mainInterface;
    private Vector3 playerVelocity;
    private float dashCooldown = 1.5f;
    public bool isDashing = false;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintValue = 2.0f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;

    //Variables relacionadas al ataque
    public bool isAttacking = false;
    public bool isCastingAbility = false;

    [SerializeField] Transform mano;
    [SerializeField] GameObject shieldPrefab;

    public Animator animator;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInventory = GetComponent<PlayerInventory>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponentInChildren<Animator>();  


        offensiveAbilityController = GetComponent<OffensiveAbility>();
        defensiveAbilityController = GetComponent<DefensiveAbility>();
        healingAbilityController = GetComponent<HealingAbility>();

        mainInterface = FindFirstObjectByType<MainInterface>();

    }

    protected virtual void Update()
    {
        if (GameManager.instance.isPaused) return;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintValue : 1f;
        Vector3 finalMove = move * playerSpeed * speedMultiplier;
        float inputMagnitude = finalMove.magnitude;
        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);
        controller.Move(finalMove * Time.deltaTime );

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDashing) StartCoroutine(Dash(move));
        }

        if (!isDashing && !isAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && playerInventory.weapon != null)
            {
                playerInventory.selectedItemType = ItemType.Arma;
                ShowWeapon(true);
                mainInterface.LightUpItem(ItemType.Arma, AbilityType.None);
            }
            if (Input.GetKeyDown(KeyCode.Q) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva))
            {
                if (offensiveAbilityController.offensiveAbilityCooldown == 0)
                {
                    playerInventory.selectedItemType = ItemType.Habilidad;
                    playerInventory.selectedAbilityType = AbilityType.Ofensiva;
                    mainInterface.LightUpItem(ItemType.Habilidad, AbilityType.Ofensiva);
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva))
            {
                if (defensiveAbilityController.defensiveAbilityCooldown == 0)
                {
                    playerInventory.selectedItemType = ItemType.Habilidad;
                    playerInventory.selectedAbilityType = AbilityType.Defensiva;
                    defensiveAbilityController.enableShield();
                    ShowWeapon(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa))
            {
                if (healingAbilityController.healingAbilityCooldown == 0)
                {
                    playerInventory.selectedItemType = ItemType.Habilidad;
                    playerInventory.selectedAbilityType = AbilityType.Curativa;
                    StartCoroutine(healingAbilityController.healingAbility());
                    ShowWeapon(true);
                }
            }

            if (playerInventory.selectedItemType == ItemType.Habilidad && playerInventory.selectedAbilityType == AbilityType.Ofensiva)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    StartCoroutine(offensiveAbilityController.offensiveAbility());
                }
            }
        }

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -1f;
        }
        else
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }


        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator Dash(Vector3 move)
    {

        float startTime = Time.time;
        isDashing = true;
        while (Time.time < startTime + dashTime)
        {
            controller.Move(move * dashSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(dashCooldown - dashTime);
        isDashing = false;
    }

    public void comprobarEnemigosEnArea(Vector3 attackPosition, float attackRadius, int damage)
    {
        Collider[] colliders = Physics.OverlapSphere(attackPosition, attackRadius);
        HashSet<EnemyHealth> uniqueEnemies = new HashSet<EnemyHealth>();

        foreach (Collider c in colliders)
        {
            EnemyHealth enemigo = c.GetComponentInParent<EnemyHealth>();
            if (enemigo != null && !uniqueEnemies.Contains(enemigo))
            {
                uniqueEnemies.Add(enemigo);
                enemigo.TakeDamage(damage);
            }
        }
    }

    public void ShowWeapon(bool value)
    {
        mano.GetChild(0).gameObject.SetActive(value);
    }

    public void ToggleShieldPrefab(bool value)
    {
        shieldPrefab.SetActive(value);
    }
}
