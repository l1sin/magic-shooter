using System;
using UnityEngine;

public class Upgrade : ScriptableObject
{
    public string Name;
    public string Description;
    public Bonus[] Bonuses;
    public Requirement[] Requirements;

    [Serializable]
    public struct Bonus
    {
        public BonusType Type;
        public BonusMath Math;
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
            if (Math == BonusMath.Percent) pc = "%";
            return $"+{Value * level}{pc} {Type}";
        }
    }

    [Serializable]
    public struct Requirement
    {
        public int UpgradeIndex;
        //public int UpgradeLevel;

        //public bool CheckRequirement(ref int[] upgrades)
        //{
        //    if (upgrades[UpgradeIndex] >= UpgradeLevel) return true;
        //    else return false;
        //}
    }
}
