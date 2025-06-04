using UnityEngine;

public class LyxController : PlayerController
{
    private Espadachin espadachin;
    private Coroutine cargaCoroutine;

    protected override void Start()
    {
        base.Start();
        espadachin = GetComponent<Espadachin>();
    }

    protected override void Update()
    {
        base.Update();

        if (playerInventory.weapon && playerInventory.selectedItemType == ItemType.Arma)
        {
            // Iniciar carga al mantener clic derecho
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                cargaCoroutine = StartCoroutine(espadachin.ChargeSword());
            }

            // Soltar clic derecho
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                if (cargaCoroutine != null)
                {
                    StopCoroutine(cargaCoroutine);
                    cargaCoroutine = null;
                }

                if (espadachin.isFullyCharged && !isAttacking && !isDashing)
                {
                    int damage = Mathf.CeilToInt(playerInventory.weapon.damage * espadachin.chargeMultiplier);
                    StartCoroutine(espadachin.SwordAttack(damage));

                    espadachin.isChargingSword = false;
                    espadachin.isFullyCharged = false;
                    espadachin.chargeTime = 0f;
                }
            }

            // Clic izquierdo para ataque normal
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!isAttacking && !isDashing && !espadachin.isFullyCharged)
                {
                    int damage = playerInventory.weapon.damage;

                    if (espadachin.isChargingSword)
                    {
                        damage = Mathf.CeilToInt(damage * 0.5f);
                    }

                    StartCoroutine(espadachin.SwordAttack(damage));

                    espadachin.isChargingSword = false;
                    espadachin.isFullyCharged = false;
                    espadachin.chargeTime = 0f;
                }
            }
        }
    }
}
