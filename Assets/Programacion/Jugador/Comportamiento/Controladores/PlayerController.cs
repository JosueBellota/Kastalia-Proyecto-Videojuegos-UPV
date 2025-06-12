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

    private bool estabaCorriendo = false;


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

        // Movement handling - Modified to sprint by default and walk when pressing Shift
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 1f : sprintValue; // Inverted the condition
        Vector3 finalMove = move * playerSpeed * speedMultiplier;
        float inputMagnitude = finalMove.magnitude;
        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);

        if (controller != null && controller.enabled && controller.gameObject.activeInHierarchy)
        {
            controller.Move(finalMove * Time.deltaTime);

            bool estaCorriendoAhora = finalMove.magnitude > 0.1f;

            if (estaCorriendoAhora && !estabaCorriendo)
            {
                SFXManager.GetInstance()?.EmpezarRunningLoop();
            }
            else if (!estaCorriendoAhora && estabaCorriendo)
            {
                SFXManager.GetInstance()?.DetenerRunningLoop();
            }

            estabaCorriendo = estaCorriendoAhora;
        }

        // Dash handling
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash(move));
        }

        if (playerInventory.weapon != null)
        {
            ShowWeapon(true);
        }

        // Weapon selection
        if (Input.GetKeyDown(KeyCode.Alpha1) && playerInventory.weapon != null)
        {
            playerInventory.selectedItemType = ItemType.Arma;
            playerInventory.selectedAbilityType = AbilityType.None;
            mainInterface.LightUpItem(ItemType.Arma, AbilityType.None);

            // Only clear abilities if we were previously using one
            if (playerInventory.selectedItemType == ItemType.Habilidad)
            {
                mainInterface.ClearAllAbilityPrefabs();
            }
        }

        // Ability selections
        if (Input.GetKeyDown(KeyCode.Alpha2) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva))
        {
            playerInventory.selectedItemType = ItemType.Habilidad;
            playerInventory.selectedAbilityType = AbilityType.Ofensiva;
            mainInterface.LightUpItem(ItemType.Habilidad, AbilityType.Ofensiva);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva))
        {
            playerInventory.selectedItemType = ItemType.Habilidad;
            playerInventory.selectedAbilityType = AbilityType.Defensiva;
            mainInterface.LightUpItem(ItemType.Habilidad, AbilityType.Defensiva);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa))
        {
            playerInventory.selectedItemType = ItemType.Habilidad;
            playerInventory.selectedAbilityType = AbilityType.Curativa;
            mainInterface.LightUpItem(ItemType.Habilidad, AbilityType.Curativa);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInventory.selectedItemType == ItemType.Habilidad)
        {
            switch (playerInventory.selectedAbilityType)
            {
                case AbilityType.Ofensiva:
                    SFXManager.GetInstance()?.ReproducirFireball();
                    StartCoroutine(UseAbilityAndSwitchBack(
                        offensiveAbilityController.offensiveAbility(),
                        AbilityType.Ofensiva
                    ));
                    break;

                case AbilityType.Defensiva:
                    SFXManager.GetInstance()?.ReproducirForceField();
                    StartCoroutine(UseAbilityAndSwitchBack(
                        defensiveAbilityController.enableShield(),
                        AbilityType.Defensiva
                    ));
                    break;

                case AbilityType.Curativa:
                    SFXManager.GetInstance()?.ReproducirCuracion();
                    StartCoroutine(UseAbilityAndSwitchBack(
                        healingAbilityController.healingAbility(),
                        AbilityType.Curativa
                    ));
                    break;
            }
        }

        // Gravity handling
        if (controller != null && controller.enabled && controller.gameObject.activeInHierarchy)
        {
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
    }

    IEnumerator Dash(Vector3 move)
    {
        float startTime = Time.time;
        isDashing = true;

        while (Time.time < startTime + dashTime)
        {
            if (controller != null && controller.enabled && controller.gameObject.activeInHierarchy)
            {
                controller.Move(move * dashSpeed * Time.deltaTime);
            }
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

    public void ToggleShieldPrefab(bool value)
    {
        shieldPrefab.SetActive(value);
    }

    private void SelectWeapon()
    {
        // Only clear abilities if we were previously using one
        bool wasUsingAbility = playerInventory.selectedItemType == ItemType.Habilidad;

        playerInventory.selectedItemType = ItemType.Arma;
        playerInventory.selectedAbilityType = AbilityType.None;
        mainInterface.LightUpItem(ItemType.Arma, AbilityType.None);
    }

    private void SelectAbility(AbilityType abilityType)
    {
        playerInventory.selectedItemType = ItemType.Habilidad;
        playerInventory.selectedAbilityType = abilityType;
        mainInterface.LightUpItem(ItemType.Habilidad, abilityType);
    }

    private void ActivateSelectedAbility()
    {
        switch (playerInventory.selectedItemType)
        {
            case ItemType.Habilidad:
                switch (playerInventory.selectedAbilityType)
                {
                    case AbilityType.Ofensiva:
                        StartCoroutine(UseAbilityAndSwitchBack(
                        offensiveAbilityController.offensiveAbility(),
                        AbilityType.Ofensiva
                        ));
                        break;
                    case AbilityType.Defensiva:
                        StartCoroutine(UseAbilityAndSwitchBack(
                            defensiveAbilityController.enableShield(),
                            AbilityType.Defensiva
                        ));
                        break;
                    case AbilityType.Curativa:
                        StartCoroutine(UseAbilityAndSwitchBack(
                            healingAbilityController.healingAbility(),
                            AbilityType.Curativa
                        ));
                        break;
                }
                break;
        }
    }

    private IEnumerator UseAbilityAndSwitchBack(IEnumerator abilityCoroutine, AbilityType abilityType)
    {
        yield return StartCoroutine(abilityCoroutine);

        mainInterface.ClearAbilityPrefab(abilityType);
        playerInventory.RemoveAbility(abilityType);

        SelectWeapon();
        ShowWeapon(true);
    }

    public void ShowWeapon(bool value)
    {
        if (mano != null && mano.childCount > 0)
        {
            // Only hide if we explicitly want to hide (value is false)
            // Otherwise, always show if we have a weapon
            bool shouldShow = value || playerInventory.weapon != null;
            mano.GetChild(0).gameObject.SetActive(shouldShow);
        }
    }
}