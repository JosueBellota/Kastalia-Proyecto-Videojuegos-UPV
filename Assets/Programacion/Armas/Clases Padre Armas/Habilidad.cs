using UnityEngine;

public abstract class Habilidad : MonoBehaviour
{
    public string Name { get; private set; }
    public AbilityType Type { get; private set; }
    public int KillCountCooldown { get; private set; }

    public Habilidad(string name, AbilityType type, int killCountCooldown)
    {
        Name = name;
        Type = type;
        this.KillCountCooldown = killCountCooldown;
    }

}
