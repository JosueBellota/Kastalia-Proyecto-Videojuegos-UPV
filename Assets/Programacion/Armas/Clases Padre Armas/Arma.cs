using UnityEngine;

public class Arma : MonoBehaviour
{
    public string Name { get; private set; }
    public WeaponType Type { get; private set; }

    public Arma(string name, WeaponType type)
    {
        this.Name = name;
        this.Type = type;
    }

    public override string ToString()
    {
        return $"{Name} ({Type})";
    }

}
