using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject DamagePopupPrefab;
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject jugador;

    private void Start()
    {
        currentHealth = maxHealth;
        jugador = GameObject.FindGameObjectWithTag("Player");

        if(EnemyManager.Instance){
            EnemyManager.Instance.RegisterEnemy();
        }
    }

    private void Update()
    {
        if (jugador) return;
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(int damage)
    {
        if(DamagePopupPrefab && currentHealth > 0) ShowDamagePopup(damage);
        if(currentHealth - damage > 0){
            currentHealth -= damage;
            StartCoroutine(FlashOnHit());
        }
        else
        {
            currentHealth = 0;
            Die();
        }
    }

    private void ShowDamagePopup(int damage)
    {
        GameObject popup = Instantiate(DamagePopupPrefab, transform.position, Quaternion.identity, transform);
        popup.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    public void SetHealth(int value){
        if(value <= 0) return;
        maxHealth = value; currentHealth = value;
    }

    IEnumerator FlashOnHit()
    {
        Renderer enemyRenderer = GetComponentInChildren<Renderer>();
        Color originalColor = enemyRenderer.material.color;
        enemyRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.color = originalColor;
    }
    private void Die()
    {
        OffensiveAbility offensiveAbilityController = jugador.GetComponent<OffensiveAbility>();
        DefensiveAbility defensiveAbilityController = jugador.GetComponent<DefensiveAbility>();
        HealingAbility healingAbilityController = jugador.GetComponent<HealingAbility>();

        if (offensiveAbilityController.offensiveAbilityCooldown > 0)
            offensiveAbilityController.offensiveAbilityCooldown -= 1;
        if (defensiveAbilityController.defensiveAbilityCooldown > 0)
            defensiveAbilityController.defensiveAbilityCooldown -= 1;
        if (healingAbilityController.healingAbilityCooldown > 0)
            healingAbilityController.healingAbilityCooldown -= 1;

        MainInterface mainInterface = FindFirstObjectByType<MainInterface>();
        if (mainInterface)
        {
            mainInterface.SubtractCooldown();
        }
        StopAllCoroutines();

        if (EnemyManager.Instance) EnemyManager.Instance.UnregisterEnemy();

        Destroy(gameObject);
    }

}