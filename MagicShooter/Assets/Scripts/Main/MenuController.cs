using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [Header("Data")]
    public int Money;
    [SerializeField] private int _upgradesPerLevel = 10;
    [SerializeField] private Texture _yanTexture;
    [SerializeField] private RawImage[] _yanIcons;

    [Header("Map")]
    [SerializeField] private MapSelector _mapSelector;
    [SerializeField] private MapShop _mapShop;

    [Header("Spells")]
    [SerializeField] private Image[] DefaultSpellLeftImage;
    [SerializeField] private Image[] DefaultSpellRightImage;
    [SerializeField] private Sprite[] SpellSprites;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _characterLevelText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI[] _moneyText;
    [SerializeField] private TextMeshProUGUI _difficultyText;
    [SerializeField] private GameObject[] _premiumButtons;
    [SerializeField] private GameObject[] _checks;
    [SerializeField] private GameObject _thanksText;

    [Header("Progress Bars")]
    [SerializeField] private ProgressBar _spellsBar;
    [SerializeField] private ProgressBar _mapsBar;
    [SerializeField] private ProgressBar _achievementsBar;
    [SerializeField] private ProgressBar _upgradesBar;

    [Header("Buttons")]
    [SerializeField] private SpellButtonMenu[] _spellButtons;

    [Header("Achievements")]
    [SerializeField] private AchievementController _achievementController;

    [Header("Sliders")]
    [SerializeField] private SliderController[] _sliders;

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
        CursorHelper.ShowCursor();
        LoadPremium();
        SetDifficultyText();
        CheckSpellUpgrades();
        LoadSliders();
        LoadDefaultSpells();
        LoadMapsInfo();
        SetLevelText();
        LoadMoney();
        LoadAchivements();

        //Last
        UpdateAllProgressBars();
        CalculateStats();
        SetYanTexture("https://yastatic.net/s3/games-static/static-data/images/payments/sdk/currency-icon-m.png");
    }

    public void BuyNoAds()
    {
        SaveManager.Instance.CurrentProgress.NoAds = true;
        _premiumButtons[0].SetActive(false);
        _checks[0].SetActive(true);
        _thanksText.SetActive(true);
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }

    public void BuyCoinPremium()
    {
        SaveManager.Instance.CurrentProgress.CoinPremium = true;
        _premiumButtons[1].SetActive(false);
        _checks[1].SetActive(true);
        _thanksText.SetActive(true);
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }

    public void BuyExpPremium()
    {
        SaveManager.Instance.CurrentProgress.ExpPremium = true;
        _premiumButtons[2].SetActive(false);
        _checks[2].SetActive(true);
        _thanksText.SetActive(true);
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }

    public void LoadPremium()
    {
        if (SaveManager.Instance.CurrentProgress.NoAds)
        {
            _premiumButtons[0].SetActive(false);
            _checks[0].SetActive(true);
            _thanksText.SetActive(true);
        }
        if (SaveManager.Instance.CurrentProgress.CoinPremium)
        {
            _premiumButtons[1].SetActive(false);
            _checks[1].SetActive(true);
            _thanksText.SetActive(true);
        }
        if (SaveManager.Instance.CurrentProgress.ExpPremium)
        {
            _premiumButtons[2].SetActive(false);
            _checks[2].SetActive(true);
            _thanksText.SetActive(true);
        } 
    }

    public void SetYanTexture(string url)
    {
        StartCoroutine(DownloadYanImage(url));
    }

    public IEnumerator DownloadYanImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            _yanTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        foreach (RawImage ri in _yanIcons)
        {
            ri.texture = _yanTexture;
        }
    }

    public void SetDifficultyText()
    {
        switch (SaveManager.Instance.CurrentProgress.Difficulty)
        {
            case 0:
                _difficultyText.text = "Difficulty: Easy";
                break;
            case 1:
                _difficultyText.text = "Difficulty: Medium";
                break;
            case 2:
                _difficultyText.text = "Difficulty: Hard";
                break;
            default: break;
        }
    }

    public void ResetLevelProgress()
    {
        SaveManager.Instance.CurrentProgress.Level = 0;
        SetLevelText();
    }

    public void SetDifficulty(int difficulty)
    {
        SaveManager.Instance.CurrentProgress.Difficulty = difficulty;
    }

    public void CheckSpellUpgrades()
    {
        UpgradeController.Instance.CheckSpellUpgrade();
    }

    public void StartLevel()
    {
        StatsController.Instance.ReCalculate();
        SaveManager.Instance.CurrentProgress.CurrentStats = StatsController.Instance._FinStats;
        SceneManager.LoadScene(SaveManager.Instance.CurrentProgress.SelectedMap + 2);
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }

    public void LoadSliders()
    {
        foreach (SliderController s in _sliders)
        {
            s.LoadSlider();
        }
    }

    public void LoadAchivements()
    {
        _achievementController.LoadAchievements();
    }

    public void CalculateStats()
    {
        StatsController.Instance.ReCalculate();
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

    public void UpdateAchievementsBar()
    {
        int j = 0;
        for (int i = 0; i < 27; i++)
        {
            if (SaveManager.Instance.CurrentProgress.Achievements[i] == true) j++;
        }
        _achievementsBar.UpdateProgressBar(j, 27);
    }
    public void UpdateUpgradesBar() 
    {
        int j = 0;
        for (int i = 0; i < SaveManager.Instance.CurrentProgress.Upgrades.Length; i++)
        {
            j += SaveManager.Instance.CurrentProgress.Upgrades[i];
        }
        _upgradesBar.UpdateProgressBar(j, SaveManager.Instance.CurrentProgress.Upgrades.Length * 10);
        SaveManager.Instance.CurrentProgress.CharacterLevel = j / _upgradesPerLevel;
        _characterLevelText.text = $"Character Level {SaveManager.Instance.CurrentProgress.CharacterLevel + 1}";
    }

    public void LoadMoney()
    {
        Money = SaveManager.Instance.CurrentProgress.Money;
        UpdateMoneyTexts();
    }

    public void SpendMoney(int spendAmount)
    {
        Money -= spendAmount;
        SaveManager.Instance.CurrentProgress.Money = Money;
        UpdateMoneyTexts();
    }

    public void UpdateMoneyTexts()
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

    public void LoadMapsInfo()
    {
        _mapShop.UpdateMapInfo();
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
