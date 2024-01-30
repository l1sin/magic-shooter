using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [Header("Magic Menu")]
    public Image MagicImage;
    public TextMeshProUGUI MagicName;
    public TextMeshProUGUI SpellType;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI ReloadTime;
    public TextMeshProUGUI ProjectileSpeed;
    public TextMeshProUGUI MagicDescription;
    public TextMeshProUGUI[] MagicBonuses;
    public TextMeshProUGUI[] MagicRequirements;
    public TextMeshProUGUI MagicLevelText;
    public TextMeshProUGUI Experience;
    public Image ExpBar;
    public GameObject SpellsLocked;
    public GameObject SpellsUnlocked;
    public Button _magicBuyButton;
    public TextMeshProUGUI _magicBuyButtonText;

    [Header("Skills Menu")]
    public Image SkillImage;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillDescription;
    public TextMeshProUGUI[] SkillBonuses;
    public TextMeshProUGUI[] SkillRequirements;
    public TextMeshProUGUI SkillLevelText;
    public GameObject SkillsLocked;
    public GameObject SkillsUnlocked;
    public Button _skillBuyButton;
    public TextMeshProUGUI _skillBuyButtonText;

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
            string req = $"{AllUpgradesList[Magics[index].Requirements[i].UpgradeIndex].Name} {Levels[index] + 1}";
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
                MagicBonuses[i].text = Magics[index].Bonuses[i].ThisToString(Levels[index]);
            }
            SkillLevelText.text = $"Level {Levels[index]}";
            Experience.text = $"{CurrentExp[index]}/{NextLevelExp[index]}";
            ExpBar.fillAmount = CurrentExp[index] / NextLevelExp[index];

            SpellsLocked.SetActive(false);
            SpellsUnlocked.SetActive(true);
        }


        _magicBuyButton.onClick.RemoveAllListeners();
        int price = (Levels[index] + 1) * 500;
        if (Levels[index] >= 10)
        {
            _magicBuyButtonText.text = "MAX";
            _magicBuyButton.interactable = false;
        }
        else
        {
            _magicBuyButtonText.text = price.ToString();
            if (MenuController.Instance.Money >= price)
            {
                _magicBuyButton.onClick.AddListener(()=> BuyMagic(index, price));
                _magicBuyButton.interactable = true;
            }
            else
            {
                _magicBuyButton.interactable = false;
            }
        }
    }

    public void SetSkill(int index, Sprite sprite)
    {
        SkillImage.sprite = sprite;
        SkillName.text = Skills[index].Name;
        SkillDescription.text = Skills[index].Description;

        foreach (TextMeshProUGUI sb in SkillBonuses)
        {
            sb.text = "";
        }
        foreach (TextMeshProUGUI sr in SkillRequirements)
        {
            sr.text = "";
        }

        for (int i = 0; i < Skills[index].Requirements.Length; i++)
        {
            string req = $"{AllUpgradesList[Skills[index].Requirements[i].UpgradeIndex].Name} {Levels[index+12] + 1}";
            SkillRequirements[i].text = req;
        }

        if (Levels[index+12] <= 0)
        {
            SkillsLocked.SetActive(true);
            SkillsUnlocked.SetActive(false);
        }
        else
        {
            for (int i = 0; i < Skills[index].Bonuses.Length; i++)
            {
                SkillBonuses[i].text = Skills[index].Bonuses[i].ThisToString(Levels[index+12]);
            }
            SkillLevelText.text = $"Level {Levels[index+12]}";

            SkillsLocked.SetActive(false);
            SkillsUnlocked.SetActive(true);
        }


        _skillBuyButton.onClick.RemoveAllListeners();
        int price = (Levels[index+12] + 1) * 500;
        if (Levels[index+12] >= 10)
        {
            _skillBuyButtonText.text = "MAX";
            _skillBuyButton.interactable = false;
        }
        else
        {
            _skillBuyButtonText.text = price.ToString();
            if (MenuController.Instance.Money >= price)
            {
                _skillBuyButton.onClick.AddListener(() => BuySkill(index, price));
                _skillBuyButton.interactable = true;
            }
            else
            {
                _skillBuyButton.interactable = false;
            }
        }
    }

    public void BuyMagic(int index, int price)
    {
        MenuController.Instance.SpendMoney(price);
        Levels[index]++;
        SetMagic(index, MagicImage.sprite);
    }

    public void BuySkill(int index, int price)
    {
        MenuController.Instance.SpendMoney(price);
        Levels[index+12]++;
        SetSkill(index, SkillImage.sprite);
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
