using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public Ability abilityData;
    public float distanciaRecolecta = 2f; // Puedes ajustar este valor según tu preferencia

    private Transform jugador;

    void Start()
    {
        // Buscar al jugador por etiqueta, ajusta si usas otra forma
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador con la etiqueta 'Player'.");
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);
        if (distancia <= distanciaRecolecta)
        {
            PlayerInventory inventario = jugador.GetComponent<PlayerInventory>();

            if (inventario && !inventario.HasAbility(abilityData))
            {
                SFXManager.GetInstance()?.ReproducirPickup();
                inventario.EquipAbility(abilityData);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Ya tienes esta habilidad equipada.");
            }
        }
    }
}
