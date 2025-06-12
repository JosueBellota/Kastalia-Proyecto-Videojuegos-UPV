using UnityEngine;

public class EventosAnimBallestero : MonoBehaviour
{
    BallesteroController controller;

    void Start()
    {
        controller = GetComponentInParent<BallesteroController>();
    }

    public void LanzarFlecha()
    {
        if (controller != null)
        {
            controller.LanzarFlecha();
        }
    }
}