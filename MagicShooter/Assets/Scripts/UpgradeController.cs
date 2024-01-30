using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController Instance;
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

    [Header("Data")]
    public Stats _Stats;
    public Magic[] Magics;
    public Skill[] Skills;
    public List<Upgrade> AllUpgradesList;
    public int[] Levels;
    public float[] CurrentExp;
    public float[] NextLevelExp;

    [Header("MagicMenu")]
    public Image MagicImage;
    public TextMeshProUGUI MagicName;
    public TextMeshProUGUI SpellType;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI ReloadTime;
    public TextMeshProUGUI ProjectileSpeed;
    public TextMeshProUGUI MagicDescription;
    public TextMeshProUGUI[] MagicBonuses;
    public TextMeshProUGUI[] MagicRequirements;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Experience;
    public Image ExpBar;
    public GameObject SpellsLocked;
    public GameObject SpellsUnlocked;

    public void Start()
    {
        AllUpgradesList.AddRange(Magics);
        AllUpgradesList.AddRange(Skills);
    }

    public void SetMagic(int index, Sprite sprite)
    {
        MagicImage.sprite = sprite;
        MagicName.text = Magics[index].Name;
        SpellType.text = $"Spell Type: {Magics[index].Element}";
        Damage.text = $"Damage: {Magics[index].Damage}";
        ReloadTime.text = $"Reload Time: {Magics[index].ReloadTime}";
        ProjectileSpeed.text = $"Projectile Speed: {Magics[index].ProjectileSpeed}";
        MagicDescription.text = Magics[index].Description;

        foreach (TextMeshProUGUI mb in MagicBonuses)
        {
            mb.text = "";
        }
        foreach (TextMeshProUGUI mr in MagicRequirements)
        {
            mr.text = "";
        }

        for (int i = 0; i < Magics[index].Requirements.Length; i++)
        {
            string req = $"{AllUpgradesList[Magics[index].Requirements[i].UpgradeIndex].Name} {Magics[index].Requirements[i].UpgradeLevel}";
            MagicRequirements[i].text = req;
        }

        if (Levels[index] <= 0)
        {
            SpellsLocked.SetActive(true);
            SpellsUnlocked.SetActive(false);
        }
        else
        {
            for (int i = 0; i < Magics[index].Bonuses.Length; i++)
            {
                MagicBonuses[i].text = Magics[index].Bonuses[i].ThisToString();
            }
            Level.text = $"Level {Levels[index]}";
            Experience.text = $"{CurrentExp[index]}/{NextLevelExp[index]}";
            ExpBar.fillAmount = CurrentExp[index] / NextLevelExp[index];

            SpellsLocked.SetActive(false);
            SpellsUnlocked.SetActive(true);
        }
    }

    public class Stats
    {
        public int Damage;
        public int Health;
        public int Luck;
        public int Speed;
        public int FireMagic;
        public int EarthMagic;
        public int WaterMagic;
        public int AirMagic;
    }

}
