using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Upgrade;

public class StatsController : MonoBehaviour
{
    public static StatsController Instance;
    public Stats _DefaultStats;
    public Stats _FinStats;
    public Stats _UBPercent;
    public Stats _UBFlat;
    public Stats _CLB;
    public TextMeshProUGUI[] UpgradeBonuses;
    public TextMeshProUGUI[] CharLevelBonuses;
    public TextMeshProUGUI[] FinalStats;
    public float _bonusPerLevel;


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
        CalculateLevelBonuses();
        CalculateFinal();
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

        CharLevelBonuses[0].text = $"Damage +{_CLB.Damage}%";
        CharLevelBonuses[1].text = $"Health +{_CLB.Health}%";
        CharLevelBonuses[2].text = $"Luck +{_CLB.Luck}%";
        CharLevelBonuses[3].text = $"Speed +{_CLB.Speed}%";

        FinalStats[0].text = $"Damage: {_FinStats.Damage * 100}%";
        FinalStats[1].text = $"Health: {_FinStats.Health}";
        FinalStats[2].text = $"Luck: {_FinStats.Luck * 100}%";
        FinalStats[3].text = $"Speed: {_FinStats.Speed}";
        FinalStats[4].text = $"Fire Magic: {_FinStats.FireMagic * 100}%";
        FinalStats[5].text = $"Earth Magic: {_FinStats.EarthMagic * 100}%";
        FinalStats[6].text = $"Water Magic: {_FinStats.WaterMagic * 100}%";
        FinalStats[7].text = $"Air Magic: {_FinStats.AirMagic * 100}%";
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
                            _UBPercent.EarthMagic += b.Value * l[i];
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
    }

    public void CalculateLevelBonuses()
    {
        _CLB = new Stats();
        _CLB.Damage = _bonusPerLevel * SaveManager.Instance.CurrentProgress.CharacterLevel;
        _CLB.Health = _bonusPerLevel * SaveManager.Instance.CurrentProgress.CharacterLevel;
        _CLB.Luck = _bonusPerLevel * SaveManager.Instance.CurrentProgress.CharacterLevel;
        _CLB.Speed = _bonusPerLevel * SaveManager.Instance.CurrentProgress.CharacterLevel;
    }

    public void CalculateFinal()
    {
        _FinStats = new Stats();

        _FinStats.Damage = (_DefaultStats.Damage + _UBFlat.Damage) * (((_UBPercent.Damage + _CLB.Damage) / 100) + 1);
        _FinStats.Health = (_DefaultStats.Health + _UBFlat.Health) * (((_UBPercent.Health + _CLB.Health) / 100) +1);
        _FinStats.Luck = (_DefaultStats.Luck + _UBFlat.Luck) * (((_UBPercent.Luck + _CLB.Luck) / 100) +1);
        _FinStats.Speed = (_DefaultStats.Speed + _UBFlat.Speed) * (((_UBPercent.Speed + _CLB.Speed) / 100) +1);
        _FinStats.FireMagic = (_DefaultStats.FireMagic + _UBFlat.FireMagic) * (((_UBPercent.FireMagic + _CLB.FireMagic) / 100) +1);
        _FinStats.EarthMagic = (_DefaultStats.EarthMagic + _UBFlat.EarthMagic) * (((_UBPercent.EarthMagic + _CLB.EarthMagic) / 100) +1);
        _FinStats.WaterMagic = (_DefaultStats.WaterMagic + _UBFlat.WaterMagic) * (((_UBPercent.WaterMagic + _CLB.WaterMagic) / 100) +1);
        _FinStats.AirMagic = (_DefaultStats.AirMagic + _UBFlat.AirMagic) * (((_UBPercent.AirMagic + _CLB.AirMagic) / 100) +1);
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
