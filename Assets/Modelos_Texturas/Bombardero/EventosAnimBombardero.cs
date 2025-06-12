using UnityEngine;

public class EventosAnimBombardero : MonoBehaviour
{
    BombarderoController controller;

    void Start()
    {
        controller = GetComponentInParent<BombarderoController>();
    }

    public void LanzarBomba()
    {
        if (controller != null)
            controller.LanzarBomba();
    }
}

