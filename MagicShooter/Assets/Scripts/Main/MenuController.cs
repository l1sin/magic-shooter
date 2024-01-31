using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [Header("Data")]
    public int Money;

    [Header("Map")]
    [SerializeField] private MapSelector _mapSelector;

    [Header("Spells")]
    [SerializeField] private Image[] DefaultSpellLeftImage;
    [SerializeField] private Image[] DefaultSpellRightImage;
    [SerializeField] private Sprite[] SpellSprites;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _characterLevelText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI[] _moneyText;

    [Header("Progress Bars")]
    [SerializeField] private ProgressBar _spellsBar;
    [SerializeField] private ProgressBar _mapsBar;
    [SerializeField] private ProgressBar _achievementsBar;
    [SerializeField] private ProgressBar _monstersBar;
    [SerializeField] private ProgressBar _upgradesBar;

    [Header("Buttons")]
    [SerializeField] private SpellButtonMenu[] _spellButtons;

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

    private void Start()
    {
        LoadDefaultSpells();
        LoadMap();
        SetLevelText();
        SetCharacterLevelText();
        LoadMoney();
        StatsController.Instance.ReCalculate();
        UpdateAllProgressBars();
    }

    public void UnlockButtons()
    {
        for (int i = 0; i < 12; i++)
        {
            if (SaveManager.Instance.CurrentProgress.Upgrades[i] > 0)
            {
                _spellButtons[i].Unlock();
            }
        }
        
    }

    public void UpdateAllProgressBars()
    {
        UpdateSpellsBar();
        UpdateMapsBar();
        UpdateAchievementsBar();
        UpdateMonstersBar();
        UpdateUpgradesBar();
    }

    public void UpdateSpellsBar()
    {
        int j = 0;
        for (int i = 0; i < 12; i++)
        {
            if (SaveManager.Instance.CurrentProgress.Upgrades[i] > 0) j++;
        }
        _spellsBar.UpdateProgressBar(j, 12);
        UnlockButtons();
    }

    public void UpdateMapsBar()
    {
        int j = 0;
        for (int i = 0; i < 4; i++)
        {
            if (SaveManager.Instance.CurrentProgress.Maps[i] == true) j++;
        }
        _mapsBar.UpdateProgressBar(j, 4);
    }

    public void UpdateAchievementsBar() { }
    public void UpdateMonstersBar() { }
    public void UpdateUpgradesBar() 
    {
        int j = 0;
        for (int i = 0; i < SaveManager.Instance.CurrentProgress.Upgrades.Length; i++)
        {
            j += SaveManager.Instance.CurrentProgress.Upgrades[i];
        }
        _upgradesBar.UpdateProgressBar(j, SaveManager.Instance.CurrentProgress.Upgrades.Length * 10);
    }

    public void LoadMoney()
    {
        Money = SaveManager.Instance.CurrentProgress.Money;
        UpdataMoneyTexts();
    }

    public void SpendMoney(int spendAmount)
    {
        Money -= spendAmount;
        UpdataMoneyTexts();
    }

    public void UpdataMoneyTexts()
    {
        foreach (TextMeshProUGUI t in _moneyText)
        {
            t.text = Money.ToString();
        }
    }

    public void SetLevelText()
    {
        _levelText.text = $"Level {SaveManager.Instance.CurrentProgress.Level + 1}";
    }

    public void SetCharacterLevelText()
    {
        _characterLevelText.text = $"Character Level {SaveManager.Instance.CurrentProgress.CharacterLevel + 1}";
    }

    public void LoadMap()
    {
        _mapSelector.SelectMap(SaveManager.Instance.CurrentProgress.SelectedMap);
    }

    public void LoadDefaultSpells()
    {
        SetDefaultSpells(SaveManager.Instance.CurrentProgress.DefaultSpellIndexLeft, 0);
        SetDefaultSpells(SaveManager.Instance.CurrentProgress.DefaultSpellIndexRight, 1);
    }

    public void SetDefaultSpells(int spellIndex, int hand)
    {
        if (hand == 0)
        {
            foreach (Image i in DefaultSpellLeftImage)
            {
                i.sprite = SpellSprites[spellIndex];
            }

        }
        else if (hand == 1)
        {
            foreach (Image i in DefaultSpellRightImage)
            {
                i.sprite = SpellSprites[spellIndex];
            }
        }
    }
}
