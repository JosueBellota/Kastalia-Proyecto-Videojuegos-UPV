using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Items/Habilidad")]
public class Ability : ScriptableObject
{
    public string abilityName;
    public AbilityType abilityType;
    public int damage;
    public float range;
    public float areaOfEffect;
    public int killCountCooldown;
    public Sprite icon;
}
