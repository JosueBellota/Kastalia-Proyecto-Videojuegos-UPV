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

    public bool isMiniBoss = false;

    private DamageFlash damageFlash;

    private void Start()
    {
        currentHealth = maxHealth;
        jugador = GameObject.FindGameObjectWithTag("Player");
        damageFlash = GetComponent<DamageFlash>();

        if (EnemyManager.Instance)
        {
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
        
        if(currentHealth - damage > 0)
        {

        if (gameObject.CompareTag("Demon"))
        {
            SFXManager.GetInstance()?.ReproducirDemonDamage();
        }
        else
        {
            SFXManager.GetInstance()?.ReproducirEnemyWounded();
        }
           
            currentHealth -= damage;
            if (damageFlash != null)
                damageFlash.Flash();
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

    public void SetHealth(int value)
    {
        if(value <= 0) return;
        maxHealth = value; 
        currentHealth = value;
    }

    private void Die()
    {
        
        StopAllCoroutines();
        
        if (isMiniBoss)
        {
            GameManager.instance.WinGame(); 
        }


        if (EnemyManager.Instance)
            EnemyManager.Instance.UnregisterEnemy();

        Destroy(gameObject);
    }
}