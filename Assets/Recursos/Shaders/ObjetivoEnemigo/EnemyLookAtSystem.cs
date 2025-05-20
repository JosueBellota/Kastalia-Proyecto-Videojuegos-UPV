using UnityEngine;

public class EnemyLookAtSystem : MonoBehaviour
{
    public float detectionRange = 15f;
    public LayerMask enemyMask;

    public Transform currentTarget;
    private Transform player;
    public bool isLocked = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'Player'.");
        }
    }

    void Update()
    {
        if (GameManager.instance.isPaused || player == null) return;

        // Desbloquear enemigo con tecla º
        if (Input.GetKeyDown(KeyCode.BackQuote)) // Tecla º
        {
            SetHighlightLock(currentTarget, false); // Quitar highlight
            isLocked = false;
            currentTarget = null;
        }

        // Fijar a un enemigo con click izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            Transform aimedEnemy = GetEnemyUnderMouse();
            if (aimedEnemy != null)
            {
                SetHighlightLock(currentTarget, false); // Quitar highlight del anterior

                currentTarget = aimedEnemy;
                isLocked = true;

                SetHighlightLock(currentTarget, true); // Mostrar highlight del nuevo
            }
        }

        // Si hay enemigo fijado, mirar hacia él
        if (currentTarget != null && isLocked)
        {
            Vector3 targetPos = new Vector3(currentTarget.position.x, player.position.y, currentTarget.position.z);
            player.LookAt(targetPos);

            // Desbloquear si se va fuera de rango
            if (Vector3.Distance(player.position, currentTarget.position) > detectionRange)
            {
                SetHighlightLock(currentTarget, false);
                isLocked = false;
                currentTarget = null;
            }
        }
    }

    Transform GetEnemyUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, enemyMask))
        {
            float distance = Vector3.Distance(player.position, hit.transform.position);
            if (distance <= detectionRange)
            {
                return hit.transform;
            }
        }

        return null;
    }

    void SetHighlightLock(Transform target, bool value)
    {
        if (target == null) return;

        EnemyHighlighter highlighter = target.GetComponentInChildren<EnemyHighlighter>();
        if (highlighter != null)
        {
            highlighter.SetLocked(value);
        }
    }
}
