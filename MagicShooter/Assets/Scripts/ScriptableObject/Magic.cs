using UnityEngine;

[CreateAssetMenu(fileName = "Magic", menuName = "Upgrade/Magic", order = 1)]
public class Magic : Upgrade
{
    public Spell.ShotType ShotType;
    public Spell.Element Element;
    public float Damage;
    public float ReloadTime;
    public float ProjectileSpeed;
}
