using Sounds;
using TMPro;
using UnityEngine;

public class Summary : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private int[] _baseReward;
    [SerializeField] private int _coinsCollected;
    [SerializeField] private Magic[] _magics;
    [SerializeField] private bool _coinAdBonus = false;
    [SerializeField] private bool _coinPremiumBonus = false;
    [SerializeField] private bool _expAdBonus = false;
    [SerializeField] private bool _expPremiumBonus = false;
    [SerializeField] private int _reward;
    [SerializeField] private float[] _expGain;

    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI _baseRewardText;
    [SerializeField] private TextMeshProUGUI _coinsCollectedText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private TextMeshProUGUI _expText;

    [Header("Bonuses")]
    [SerializeField] private GameObject _coinAdBonusObj;
    [SerializeField] private GameObject _coinPremiumBonusObj;
    [SerializeField] private GameObject _expAdBonusObj;
    [SerializeField] private GameObject _expPremiumBonusObj;

    [Header("Buttons")]
    [SerializeField] private GameObject _coinAdButton;
    [SerializeField] private GameObject _expAdButton;

    public void Start()
    {
        CheckPremium();
        ShowReward();
        ShowExperience();
        Save();
    }

    private void CheckPremium()
    {
        if (SaveManager.Instance.CurrentProgress.CoinPremium)
        {
            _coinPremiumBonus = true;
            _coinPremiumBonusObj.SetActive(true);
        }
        if (SaveManager.Instance.CurrentProgress.ExpPremium)
        {
            _expPremiumBonus = true;
            _expPremiumBonusObj.SetActive(true);
        }
    }

    private void ShowReward()
    {
        _reward = 0;
        int level = SaveManager.Instance.CurrentProgress.Level;
        if (level >= _baseReward.Length) level = _baseReward.Length - 1;
        _baseRewardText.text = $"{DataController.Instance.Dictionary[209]}: {_baseReward[level]}";
        int coinsCollected = LevelController.Instance.CoinsCollected;
        _coinsCollectedText.text = $"{DataController.Instance.Dictionary[210]}: {coinsCollected}";
        _reward = _baseReward[level] + coinsCollected;
        if (_coinPremiumBonus) _reward *= 2;
        if (_coinAdBonus) _reward *= 2;
        _rewardText.text = $"{DataController.Instance.Dictionary[211]}: {_reward}";
    }

    private void ShowExperience()
    {
        string exp = $"{DataController.Instance.Dictionary[212]}:\n";
        for (int i = 0; i < 12; i++)
        {
            float gain = LevelController.Instance.ExperienceOnLevel[i];
            if (_expPremiumBonus) gain *= 2;
            if (_expAdBonus) gain *= 2;
            _expGain[i] = gain;
            if (gain > 0)
            {
                exp += $"{DataController.Instance.Dictionary[_magics[i].NameId]}: +{Mathf.FloorToInt(gain)} {DataController.Instance.Dictionary[213]}\n";
            }
        }
        _expText.text = exp;
    }

    public void CallWatchCoinAd()
    {
        SoundManager.Instance.OffSound();
#if UNITY_EDITOR || UNITY_STANDALONE
        Debug.Log("Rewarded double coins");
        DoubleCoin();
#elif UNITY_WEBGL
        Yandex.WatchAdCoins();
#endif
    }

    public void CallWatchExpAd()
    {
        SoundManager.Instance.OffSound();
#if UNITY_EDITOR || UNITY_STANDALONE
        Debug.Log("Rewarded double exp");
        DoubleExp();
#elif UNITY_WEBGL
        Yandex.WatchAdExp();
#endif
    }

    public void DoubleCoin()
    {
        _coinAdBonus = true;
        _coinAdBonusObj.SetActive(true);
        _rewardText.text = $"{DataController.Instance.Dictionary[211]}: {_reward * 2}";
        SaveManager.Instance.CurrentProgress.Money += _reward;
        SaveManager.Instance.CurrentProgress.AllMoney += _reward;
        HideAdButtons();
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }
    public void DoubleExp()
    {
        _expAdBonus = true;
        _expAdBonusObj.SetActive(true);
        for (int i = 0; i < 12; i++)
        {
            SaveManager.Instance.CurrentProgress.Experience[i] += _expGain[i];
        }
        ShowExperience(); 
        HideAdButtons();
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }

    public void HideAdButtons()
    {
        _coinAdButton.SetActive(false);
        _expAdButton.SetActive(false);
    }

    public void Save()
    {
        SaveManager.Instance.CurrentProgress.Money += _reward;
        SaveManager.Instance.CurrentProgress.AllMoney += _reward;
        SaveManager.Instance.CurrentProgress.Kills += EnemySpawner.AllEnemiesCount;
        SaveManager.Instance.CurrentProgress.Level++;
        for (int i = 0; i < 12; i++)
        {
            SaveManager.Instance.CurrentProgress.Experience[i] += _expGain[i];
        }
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }

    public void Continue()
    {
        LevelController.Instance.LoadMenu();
    }
}
