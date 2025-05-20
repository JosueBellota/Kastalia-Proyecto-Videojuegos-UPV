using System.Collections.Generic;
using UnityEngine;

public static class ItemDropTracker
{
    public static HashSet<string> itemsYaSoltados = new HashSet<string>();

    public static bool YaSalio(GameObject item)
    {
        return itemsYaSoltados.Contains(item.name);
    }

    public static void Registrar(GameObject item)
    {
        itemsYaSoltados.Add(item.name);
    }

    public static void Reiniciar()
    {
        itemsYaSoltados.Clear();
    }
}
