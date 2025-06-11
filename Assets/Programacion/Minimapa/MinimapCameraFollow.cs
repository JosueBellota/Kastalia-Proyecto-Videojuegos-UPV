using UnityEngine;
using System.Collections;

public class MinimapCameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 initialCameraPosition;
    private Vector3 initialPlayerPosition;

    void Start()
    {
        initialCameraPosition = transform.position;
        StartCoroutine(FindPlayerRoutine());
    }

    IEnumerator FindPlayerRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            SetTarget(playerObj.transform);
        }
        else
        {
            Debug.LogWarning("MinimapCameraFollow: No object with tag 'Player' found.");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 offset = player.position - initialPlayerPosition;
            transform.position = new Vector3(
                initialCameraPosition.x + offset.x,
                initialCameraPosition.y, // Keep Y unchanged
                initialCameraPosition.z + offset.z
            );
        }
    }

    public void SetTarget(Transform target)
    {
        player = target;
        if (player != null)
        {
            initialPlayerPosition = player.position;
        }
    }
}
