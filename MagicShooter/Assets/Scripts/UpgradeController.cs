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
    public Magic[] Magics;
    public Skill[] Skills;
    public List<Upgrade> AllUpgradesList;
    public int[] Levels;
    public int maxLevel = 10;

    [Header("Magic Menu")]
    public GameObject MagicIcons;
    public GameObject MagicInfo;
    public Image MagicImage;
    public TextMeshProUGUI MagicName;
    public TextMeshProUGUI SpellType;
    public TextMeshProUGUI Damage;
    public TextMeshProUGUI ReloadTime;
    public TextMeshProUGUI ProjectileSpeed;
    public TextMeshProUGUI MagicDescription;
    public TextMeshProUGUI MagicRequirementText;
    public TextMeshProUGUI[] MagicBonuses;
    public TextMeshProUGUI[] MagicRequirements;
    public TextMeshProUGUI MagicLevelText;
    public GameObject SpellsLocked;
    public GameObject SpellsUnlocked;
    public Button _magicBuyButton;
    public TextMeshProUGUI _magicBuyButtonText;
    public ProgressBar _magicProgressBar;
    private int _magicLastUsedIndex = 0;
    [SerializeField] private Sprite _magicLastUsedSprite;

    [Header("Skills Menu")]
    public GameObject SkillIcons;
    public GameObject SkillInfo;
    public Image SkillImage;
    public TextMeshProUGUI SkillName;
    public TextMeshProUGUI SkillDescription;
    public TextMeshProUGUI SkillRequirementText;
    public TextMeshProUGUI[] SkillBonuses;
    public TextMeshProUGUI[] SkillRequirements;
    public TextMeshProUGUI SkillLevelText;
    public GameObject SkillsLocked;
    public GameObject SkillsUnlocked;
    public Button _skillBuyButton;
    public TextMeshProUGUI _skillBuyButtonText;
    private int _skillLastUsedIndex = 0;
    [SerializeField] private Sprite _skillLastUsedSprite;

    [Header("Other")]
    public bool _lastUsedMenu = false;
    public Color DefaultColor;
    public Color GoodColor;
    public Color BadColor;

    public void Start()
    {
        Levels = SaveManager.Instance.CurrentProgress.Upgrades;

        AllUpgradesList.AddRange(Magics);
        AllUpgradesList.AddRange(Skills);
    }

    public void ShowMenu()
    {
        if (!_lastUsedMenu) ShowSpells();
        else ShowSkill();
    }
    public void ShowSpells()
    {
        MagicIcons.SetActive(true);
        MagicInfo.SetActive(true);
        SkillIcons.SetActive(false);
        SkillInfo.SetActive(false);

        SetMagic(_magicLastUsedIndex, _magicLastUsedSprite);
    }
    public void ShowSkill()
    {
        MagicIcons.SetActive(false);
        MagicInfo.SetActive(false);
        SkillIcons.SetActive(true);
        SkillInfo.SetActive(true);

        SetSkill(_skillLastUsedIndex, _skillLastUsedSprite);
    }

    public void SetMagic(int index, Sprite sprite)
    {
        if (sprite == null) sprite = _magicLastUsedSprite;
        _magicLastUsedSprite = sprite;
        _magicLastUsedIndex = index;
        _lastUsedMenu = false;
        bool allowedToBuy = true;

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
            mr.color = DefaultColor;
            mr.text = "";
        }

        if (Levels[index] >= maxLevel)
        {
            MagicRequirementText.text = "Max level reached";
        }
        else
        {
            MagicRequirementText.text = "Requirements:";

            if (Magics[index].Requirements.Length > 0)
            {
                for (int i = 0; i < Magics[index].Requirements.Length; i++)
                {
                    int reqIndex = Magics[index].Requirements[i].UpgradeIndex;
                    string req = $"{AllUpgradesList[reqIndex].Name} {Levels[index] + 1}";

                    if (Levels[reqIndex] >= Levels[index] + 1)
                    {
                        allowedToBuy = true;
                        MagicRequirements[i].color = GoodColor;
                    }
                    else
                    {
                        allowedToBuy = false;
                        MagicRequirements[i].color = BadColor;
                    }

                    MagicRequirements[i].text = req;
                }
            }
            else
            {
                MagicRequirements[0].text = "No requirements";
            }
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
            MagicLevelText.text = $"Level {Levels[index]}";

            if (Levels[index] < maxLevel)
            {
                float CurrentExperience = Mathf.Floor(SaveManager.Instance.CurrentProgress.Experience[index]);
                float ExperienceToLevelUp = DataController.Instance.GetExperienceValue(index, Levels[index]);
                _magicProgressBar.UpdateProgressBar(CurrentExperience, ExperienceToLevelUp);
            }
            else
            {
                _magicProgressBar.UpdateProgressBar(1, 1);
            }

            SpellsLocked.SetActive(false);
            SpellsUnlocked.SetActive(true);
        }


        _magicBuyButton.onClick.RemoveAllListeners();
        if (allowedToBuy)
        {
            int price = DataController.Instance.GetPriceValue(index, Levels[index]);
            if (Levels[index] >= maxLevel)
            {
                _magicBuyButtonText.text = "MAX";
                _magicBuyButton.interactable = false;
            }
            else
            {
                _magicBuyButtonText.text = price.ToString();
                if (MenuController.Instance.Money >= price)
                {
                    _magicBuyButton.onClick.AddListener(() => BuyMagic(index, price));
                    _magicBuyButton.interactable = true;
                }
                else
                {
                    _magicBuyButton.interactable = false;
                }
            }
        }
        else
        {
            _magicBuyButtonText.text = "Requirements not met";
            _magicBuyButton.interactable = false;
        } 
    }

    public void SetSkill(int index, Sprite sprite)
    {
        if (sprite == null) sprite = _skillLastUsedSprite;
        _skillLastUsedSprite = sprite;
        _skillLastUsedIndex = index;
        _lastUsedMenu = true;
        bool allowedToBuy = true;

        SkillImage.sprite = sprite;
        SkillName.text = Skills[index].Name;
        SkillDescription.text = Skills[index].Description;

        foreach (TextMeshProUGUI sb in SkillBonuses)
        {
            sb.text = "";
        }
        foreach (TextMeshProUGUI sr in SkillRequirements)
        {
            sr.color = DefaultColor;
            sr.text = "";
        }

        if (Levels[index + 12] >= maxLevel)
        {
            SkillRequirementText.text = "Max level reached";
        }
        else
        {
            SkillRequirementText.text = "Requirements:";

            if (Skills[index].Requirements.Length > 0)
            {
                for (int i = 0; i < Skills[index].Requirements.Length; i++)
                {
                    int reqIndex = Skills[index].Requirements[i].UpgradeIndex;
                    string req = $"{AllUpgradesList[reqIndex].Name} {Levels[index + 12] + 1}";

                    if (Levels[reqIndex] >= Levels[index + 12] + 1)
                    {
                        allowedToBuy = true;
                        SkillRequirements[i].color = GoodColor;
                    }
                    else
                    {
                        allowedToBuy = false;
                        SkillRequirements[i].color = BadColor;
                    }

                    SkillRequirements[i].text = req;
                }
            }
            else
            {
                SkillRequirements[0].text = "No requirements";
            }
        }
        

        if (Levels[index + 12] <= 0)
        {
            SkillsLocked.SetActive(true);
            SkillsUnlocked.SetActive(false);
        }
        else
        {
            for (int i = 0; i < Skills[index].Bonuses.Length; i++)
            {
                SkillBonuses[i].text = Skills[index].Bonuses[i].ThisToString(Levels[index + 12]);
            }
            SkillLevelText.text = $"Level {Levels[index + 12]}";

            SkillsLocked.SetActive(false);
            SkillsUnlocked.SetActive(true);
        }


        _skillBuyButton.onClick.RemoveAllListeners();
        int price = DataController.Instance.GetPriceValue(index + 12, Levels[index + 12]);
        if (allowedToBuy)
        {
            if (Levels[index + 12] >= maxLevel)
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
        else
        {
            _skillBuyButtonText.text = "Requirements not met";
            _skillBuyButton.interactable = false;
        }   
    }

    public void BuyMagic(int index, int price)
    {
        MenuController.Instance.SpendMoney(price);
        Levels[index]++;
        SetMagic(index, MagicImage.sprite);
        MenuController.Instance.UpdateUpgradesBar();
        MenuController.Instance.UpdateSpellsBar();
        SaveManager.Instance.CurrentProgress.Upgrades = Levels;
        AchievementController.Instance.CheckType(AchievementController.Achievement.AchievementType.Upgrades);
        AchievementController.Instance.CheckType(AchievementController.Achievement.AchievementType.Spells);
    }

    public void BuySkill(int index, int price)
    {
        MenuController.Instance.SpendMoney(price);
        Levels[index + 12]++;
        SetSkill(index, SkillImage.sprite);
        MenuController.Instance.UpdateUpgradesBar();
        SaveManager.Instance.CurrentProgress.Upgrades = Levels;
        AchievementController.Instance.CheckType(AchievementController.Achievement.AchievementType.Upgrades);
    }

    public void CheckSpellUpgrade()
    {
        for (int i = 0; i < 12; i++)
        {
            if (SaveManager.Instance.CurrentProgress.Upgrades[i] < maxLevel)
            {
                bool canLevelup = true;
                while (canLevelup)
                {
                    if (SaveManager.Instance.CurrentProgress.Experience[i] > DataController.Instance.GetExperienceValue(i, SaveManager.Instance.CurrentProgress.Upgrades[i]))
                    {
                        if (CheckMagicReq(i))
                        {
                            SaveManager.Instance.CurrentProgress.Experience[i] -= DataController.Instance.GetExperienceValue(i, SaveManager.Instance.CurrentProgress.Upgrades[i]);
                            SaveManager.Instance.CurrentProgress.Upgrades[i]++;
                        }
                        else
                        {
                            SaveManager.Instance.CurrentProgress.Experience[i] = DataController.Instance.GetExperienceValue(i, SaveManager.Instance.CurrentProgress.Upgrades[i]) - 1;
                            canLevelup = false;
                        }
                    }
                    else canLevelup = false;
                }
            }
            else
            {
                SaveManager.Instance.CurrentProgress.Experience[i] = 1;
            }
        }
    }

    public bool CheckMagicReq(int index)
    {
        bool reqMet = true;

        if (Magics[index].Requirements.Length > 0)
        {
            for (int i = 0; i < Magics[index].Requirements.Length; i++)
            {
                int reqIndex = Magics[index].Requirements[i].UpgradeIndex;

                if (Levels[reqIndex] >= Levels[index] + 1)
                {
                    continue;
                }
                else
                {
                    reqMet = false;
                }
            }
        }
        return reqMet;
    }
}
