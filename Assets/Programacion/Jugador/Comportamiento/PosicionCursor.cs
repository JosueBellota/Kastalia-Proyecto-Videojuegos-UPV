using UnityEngine;

public class PosicionCursor : MonoBehaviour
{
    public Vector3 lookPoint;
    private Transform player;
    private LayerMask layerMask;
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontr� un objeto con la etiqueta 'Player'.");
        }

        layerMask = ~LayerMask.GetMask("Player", "Paredes");
    }

    void Update()
    {
        if(GameManager.instance.isPaused) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 worldPoint = hit.point;
            Vector3 Yremoved = new Vector3(worldPoint.x, player.position.y, worldPoint.z);
            lookPoint = Yremoved;
            player.LookAt(Yremoved);
        }
        if (player == null) return;

    }
}
