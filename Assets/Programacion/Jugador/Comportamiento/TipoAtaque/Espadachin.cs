using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espadachin : MonoBehaviour
{
    [SerializeField] float attackrange = 2f;
    [SerializeField] float chargeRequiredTime = 1f;
    public float chargeMultiplier = 3f;
    public float chargeTime = 0f;
    public bool isChargingSword = false;
    public bool isFullyCharged = false;
    public bool isRightMouseDown = false;
    private LyxController controller;
    private PlayerInventory playerInventory;
    private Animator animator;
    private Vector3 attackDamageOffset;
    [SerializeField] private float empujeFuerza = 5f;
    public float attackCooldown = 0.5f;
    private bool canAttack = true;

    void Start()
    {
        controller = GetComponent<LyxController>();
        playerInventory = GetComponent<PlayerInventory>();
        animator = GetComponentInChildren<Animator>();
        attackDamageOffset = new Vector3(0, 0, 1f);
        
        // Set animator to update immediately
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerInventory.selectedItemType == ItemType.Arma && canAttack)
        {
            if (controller.isAttacking) return;

            if (playerInventory == null || playerInventory.weapon == null)
            {
                Debug.LogWarning("Player inventory or weapon not set!");
                return;
            }

            int damage = playerInventory.weapon.damage;
            animator.SetTrigger("AtaqueLigero"); // Use trigger instead of Play()
            StartCoroutine(SwordAttack(damage));
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseDown = false;

            if (isFullyCharged && !controller.isAttacking && playerInventory != null && playerInventory.weapon != null && canAttack)
            {
                int baseDamage = playerInventory.weapon.damage;
                int damage = Mathf.CeilToInt(baseDamage * chargeMultiplier);
                animator.SetTrigger("AtaquePesado"); // Use trigger instead of Play()
                StartCoroutine(SwordAttack(damage));
                attackCooldown = 1f;
                isFullyCharged = false;
            }
        }
    }
    public IEnumerator ChargeSword()
    {
        if (isChargingSword)
            yield break;

        isChargingSword = true;
        animator.SetBool("Cargando", true);
        isFullyCharged = false;
        chargeTime = 0f;
        animator.SetFloat("Fuerza", 0);

        while (isRightMouseDown && chargeTime < chargeRequiredTime)
        {
            chargeTime += Time.deltaTime;
            animator.SetFloat("Fuerza", chargeTime / chargeRequiredTime);
            yield return null;
        }

        if (chargeTime >= chargeRequiredTime)
        {
            isFullyCharged = true;
            animator.SetFloat("Fuerza", 1);
            animator.SetTrigger("ReleaseClick");
        }
        animator.SetBool("Cargando", false);
        isChargingSword = false;
    }

    private void EmpujarEnemigo(GameObject enemigo, float fuerzaEmpuje)
    {
        Vector3 direccionEmpuje = (enemigo.transform.position - transform.position).normalized;
        direccionEmpuje.y = 0;
        StartCoroutine(EmpujeSuave(enemigo.transform, direccionEmpuje, fuerzaEmpuje));
    }

    private IEnumerator EmpujeSuave(Transform objetivo, Vector3 direccion, float fuerza)
    {
        if (!objetivo) yield break;
        float duracionEmpuje = 0.5f;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionEmpuje)
        {
            if (!objetivo) yield break;
            float progreso = 1 - (tiempoTranscurrido / duracionEmpuje);
            objetivo.position += direccion * fuerza * progreso * Time.deltaTime;
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator SwordAttack(int damage)
    {
        canAttack = false;
        controller.isAttacking = true;

        try
        {
            // Apply damage immediately (not waiting for animation)
            Vector3 attackPosition = controller.transform.position +
                                     controller.transform.forward * attackDamageOffset.z +
                                     controller.transform.up * attackDamageOffset.y +
                                     controller.transform.right * attackDamageOffset.x;

            Collider[] colliders = Physics.OverlapSphere(attackPosition, attackrange);
            HashSet<EnemyHealth> uniqueEnemies = new HashSet<EnemyHealth>();

            foreach (Collider c in colliders)
            {
                EnemyHealth enemigo = c.GetComponentInParent<EnemyHealth>();
                if (enemigo != null && !uniqueEnemies.Contains(enemigo))
                {
                    uniqueEnemies.Add(enemigo);
                    enemigo.TakeDamage(damage);

                    float fuerzaEmpujeActual = isFullyCharged ? empujeFuerza * 1.5f : empujeFuerza;
                    EmpujarEnemigo(enemigo.gameObject, fuerzaEmpujeActual);
                }
            }

            // Wait for cooldown (but animation plays independently)
            yield return new WaitForSeconds(attackCooldown);
        }
        finally
        {
            controller.isAttacking = false;
            canAttack = true;
        }
    }
}