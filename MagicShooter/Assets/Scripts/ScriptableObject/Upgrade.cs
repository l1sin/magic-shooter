using System;
using UnityEngine;

public class Upgrade : ScriptableObject
{
    public int NameId;
    public int DescriptionId;
    public Bonus[] Bonuses;
    public Requirement[] Requirements;
    

    [Serializable]
    public struct Bonus
    {
        public BonusType _BonusType;
        public BonusMath _BonusMath;
        public float Value;

        public enum BonusType
        {
            Damage,
            Health,
            Luck,
            Speed,
            FireMagic,
            EarthMagic,
            WaterMagic,
            AirMagic
        }

        public enum BonusMath
        {
            Flat,
            Percent
        }

        public string ThisToString(int level)
        {
            string pc = string.Empty;
            if (_BonusMath == BonusMath.Percent) pc = "%";
            return $"+{Value * level}{pc} {GetTypeName()}";
        }

        public string GetTypeName()
        {
            switch (_BonusType)
            {
                case BonusType.Damage:
                    return DataController.Instance.Dictionary[IndexHolder.DamageNameId];
                case BonusType.Health:
                    return DataController.Instance.Dictionary[IndexHolder.HealthNameId];
                case BonusType.Luck:
                    return DataController.Instance.Dictionary[IndexHolder.LuckNameId];
                case BonusType.Speed:
                    return DataController.Instance.Dictionary[IndexHolder.SpeedNameId];
                case BonusType.FireMagic:
                    return DataController.Instance.Dictionary[IndexHolder.FireMagicNameId];
                case BonusType.EarthMagic:
                    return DataController.Instance.Dictionary[IndexHolder.EarthMagicNameId];
                case BonusType.WaterMagic:
                    return DataController.Instance.Dictionary[IndexHolder.WaterMagicNameId];
                case BonusType.AirMagic:
                    return DataController.Instance.Dictionary[IndexHolder.AirMagicNameId];
                default: return "";
            }
        }
    }
    

    [Serializable]
    public struct Requirement
    {
        public int UpgradeIndex;
    }
}
