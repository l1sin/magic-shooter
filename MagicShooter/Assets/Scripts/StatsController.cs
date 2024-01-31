using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Upgrade;

public class StatsController : MonoBehaviour
{
    public static StatsController Instance;
    public Stats _DefaultStats = new Stats();
    public Stats _FinStats = new Stats();
    public Stats _UBPercent = new Stats();
    public Stats _UBFlat = new Stats();
    public Stats _CLB = new Stats();
    public TextMeshProUGUI[] UpgradeBonuses;
    public TextMeshProUGUI[] CharLevelBonuses;
    public TextMeshProUGUI[] FinalStats;


    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ReCalculate()
    {
        CalculateBonuses();
        UpdateStats();
    }

    public void UpdateStats()
    {
        UpgradeBonuses[0].text = $"Damage +{_UBPercent.Damage}%";
        UpgradeBonuses[1].text = $"Health +{_UBFlat.Health}";
        UpgradeBonuses[2].text = $"Health +{_UBPercent.Health}%";
        UpgradeBonuses[3].text = $"Luck +{_UBPercent.Luck}%";
        UpgradeBonuses[4].text = $"Speed +{_UBPercent.Speed}%";
        UpgradeBonuses[5].text = $"Fire Magic +{_UBPercent.FireMagic}%";
        UpgradeBonuses[6].text = $"Earth Magic +{_UBPercent.EarthMagic}%";
        UpgradeBonuses[7].text = $"Water Magic +{_UBPercent.WaterMagic}%";
        UpgradeBonuses[8].text = $"Air Magic +{_UBPercent.AirMagic}%";

        //CharLevelBonuses[0].text = $"Damage {}";
        //CharLevelBonuses[1].text = $"Health {}";
        //CharLevelBonuses[2].text = $"Luck {}";
        //CharLevelBonuses[3].text = $"Speed {}";

        //FinalStats[0].text = $"Damage {}";
        //FinalStats[1].text = $"Health {}";
        //FinalStats[2].text = $"Luck {}";
        //FinalStats[3].text = $"Speed {}";
    }

    public void CalculateBonuses()
    {
        _UBPercent = new Stats();
        _UBFlat = new Stats();

        List<Upgrade> u = UpgradeController.Instance.AllUpgradesList;
        int[] l = UpgradeController.Instance.Levels;

        for (int i = 0; i < u.Count; i++)
        {
            foreach (Bonus b in u[i].Bonuses)
            {
                if (b.Math == Bonus.BonusMath.Percent)
                {
                    switch (b.Type)
                    {
                        case Bonus.BonusType.Damage:
                            _UBPercent.Damage += b.Value * l[i];
                            break;
                        case Bonus.BonusType.Health:
                            _UBPercent.Health += b.Value * l[i];
                            break;
                        case Bonus.BonusType.Luck:
                            _UBPercent.Luck += b.Value * l[i];
                            break;
                        case Bonus.BonusType.Speed:
                            _UBPercent.Speed += b.Value * l[i];
                            break;
                        case Bonus.BonusType.FireMagic:
                            _UBPercent.FireMagic += b.Value * l[i];
                            break;
                        case Bonus.BonusType.EarthMagic:
                            _UBPercent.EarthMagic += b.Value* l[i];
                            break;
                        case Bonus.BonusType.WaterMagic:
                            _UBPercent.WaterMagic += b.Value * l[i];
                            break;
                        case Bonus.BonusType.AirMagic:
                            _UBPercent.AirMagic += b.Value * l[i];
                            break;
                        default: break;
                    }
                }
                else if (b.Math == Bonus.BonusMath.Flat)
                {
                    switch (b.Type)
                    {
                        case Bonus.BonusType.Damage:
                            _UBFlat.Damage += b.Value * l[i];
                            break;
                        case Bonus.BonusType.Health:
                            _UBFlat.Health += b.Value * l[i];
                            break;
                        case Bonus.BonusType.Luck:
                            _UBFlat.Luck += b.Value * l[i];
                            break;
                        case Bonus.BonusType.Speed:
                            _UBFlat.Speed += b.Value * l[i];
                            break;
                        case Bonus.BonusType.FireMagic:
                            _UBFlat.FireMagic += b.Value * l[i];
                            break;
                        case Bonus.BonusType.EarthMagic:
                            _UBFlat.EarthMagic += b.Value * l[i];
                            break;
                        case Bonus.BonusType.WaterMagic:
                            _UBFlat.WaterMagic += b.Value * l[i];
                            break;
                        case Bonus.BonusType.AirMagic:
                            _UBFlat.AirMagic += b.Value * l[i];
                            break;
                        default: break;
                    }
                }
            }
        }

        //foreach (Upgrade u in UpgradeController.Instance.AllUpgradesList)
        //{
        //    foreach (Bonus b in u.Bonuses)
        //    {
        //        if (b.Math == Bonus.BonusMath.Percent)
        //        {
        //            switch (b.Type)
        //            {
        //                case Bonus.BonusType.Damage:
        //                    _UBPercent.Damage += b.Value / 100 * ;
        //                    break;
        //                case Bonus.BonusType.Health:
        //                    _UBPercent.Health += b.Value / 100;
        //                    break;
        //                case Bonus.BonusType.Luck:
        //                    _UBPercent.Luck += b.Value / 100;
        //                    break;
        //                case Bonus.BonusType.Speed:
        //                    _UBPercent.Speed += b.Value / 100;
        //                    break;
        //                case Bonus.BonusType.FireMagic:
        //                    _UBPercent.FireMagic += b.Value / 100;
        //                    break;
        //                case Bonus.BonusType.EarthMagic:
        //                    _UBPercent.EarthMagic += b.Value / 100;
        //                    break;
        //                case Bonus.BonusType.WaterMagic:
        //                    _UBPercent.WaterMagic += b.Value / 100;
        //                    break;
        //                case Bonus.BonusType.AirMagic:
        //                    _UBPercent.AirMagic += b.Value / 100;
        //                    break;
        //                default: break;
        //            }
        //        }
        //        else if (b.Math == Bonus.BonusMath.Flat)
        //        {
        //            switch (b.Type)
        //            {
        //                case Bonus.BonusType.Damage:
        //                    _UBFlat.Damage += b.Value;
        //                    break;
        //                case Bonus.BonusType.Health:
        //                    _UBFlat.Health += b.Value;
        //                    break;
        //                case Bonus.BonusType.Luck:
        //                    _UBFlat.Luck += b.Value;
        //                    break;
        //                case Bonus.BonusType.Speed:
        //                    _UBFlat.Speed += b.Value;
        //                    break;
        //                case Bonus.BonusType.FireMagic:
        //                    _UBFlat.FireMagic += b.Value;
        //                    break;
        //                case Bonus.BonusType.EarthMagic:
        //                    _UBFlat.EarthMagic += b.Value;
        //                    break;
        //                case Bonus.BonusType.WaterMagic:
        //                    _UBFlat.WaterMagic += b.Value;
        //                    break;
        //                case Bonus.BonusType.AirMagic:
        //                    _UBFlat.AirMagic += b.Value;
        //                    break;
        //                default: break;
        //            }
        //        }


        //    }
        //}
    }

    [Serializable]
    public class Stats
    {
        public float Damage;
        public float Health;
        public float Luck;
        public float Speed;
        public float FireMagic;
        public float EarthMagic;
        public float WaterMagic;
        public float AirMagic;

        public Stats()
        {
            Damage = 0;
            Health = 0;
            Luck = 0;
            Speed = 0;
            FireMagic = 0;
            EarthMagic = 0;
            WaterMagic = 0;
            AirMagic = 0;
        }
    }
}
