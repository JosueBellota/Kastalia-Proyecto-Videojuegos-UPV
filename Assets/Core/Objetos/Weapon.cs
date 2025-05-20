using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Arma")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    public int damage;
    public Sprite icon;
}
