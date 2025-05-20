using UnityEngine;

public class DrevenController : PlayerController
{
    private Arquero arquero;

    protected override void Start()
    {
        base.Start();
        arquero = GetComponent<Arquero>();
    }
    protected override void Update()
    {
        base.Update();
        if(playerInventory.selectedItemType == ItemType.Arma && playerInventory.weapon){
            if (Input.GetMouseButtonDown(0)) arquero.DisparoLigero();
            if (Input.GetMouseButton(1)) arquero.EmpezarCarga();
            if (Input.GetMouseButtonUp(1)) arquero.SoltarCarga();
        }
    }
}
