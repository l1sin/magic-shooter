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
        LoadMoney();
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
