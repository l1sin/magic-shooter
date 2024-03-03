using UnityEngine;

[CreateAssetMenu(fileName = "Magic", menuName = "Upgrade/Magic", order = 1)]
public class Magic : Upgrade
{
    public Spell.ShotType ShotType;
    public Spell.Element Element;
    public float Damage;
    public float ReloadTime;
    public float ProjectileSpeed;

    public string GetElementName()
    {
        switch (Element)
        {
            case Spell.Element.Fire:
                return DataController.Instance.Dictionary[IndexHolder.FireNameId];
            case Spell.Element.Earth:
                return DataController.Instance.Dictionary[IndexHolder.EarthNameId];
            case Spell.Element.Water:
                return DataController.Instance.Dictionary[IndexHolder.WaterNameId];
            case Spell.Element.Air:
                return DataController.Instance.Dictionary[IndexHolder.AirNameId];
            default: return "";
        }
    }
}
