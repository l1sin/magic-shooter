using System;
using System.Collections;
using System.Collections.Generic;
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
        _baseRewardText.text = $"Base Reward: {_baseReward[level]}";
        int coinsCollected = LevelController.Instance.CoinsCollected;
        _coinsCollectedText.text = $"Coins collected: {coinsCollected}";
        _reward = _baseReward[level] + coinsCollected;
        if (_coinPremiumBonus) _reward *= 2;
        if (_coinAdBonus) _reward *= 2;
        _rewardText.text = $"Reward: {_reward}";
    }

    private void ShowExperience()
    {
        string exp = "Experience:\n";
        for (int i = 0; i < 12; i++)
        {
            float gain = LevelController.Instance.ExperienceOnLevel[i];
            if (_expPremiumBonus) gain *= 2;
            if (_expAdBonus) gain *= 2;
            _expGain[i] = gain;
            if (gain > 0)
            {
                exp += $"{_magics[i].NameId}: +{Mathf.FloorToInt(gain)} Exp\n";
            }
        }
        _expText.text = exp;
    }

    public void DoubleCoin()
    {
        _coinAdBonus = true;
        _coinAdBonusObj.SetActive(true);
        ShowReward();
        HideAdButtons();
    }
    public void DoubleExp()
    {
        _expAdBonus = true;
        _expAdBonusObj.SetActive(true);
        ShowExperience();
        HideAdButtons();
    }

    public void HideAdButtons()
    {
        _coinAdButton.SetActive(false);
        _expAdButton.SetActive(false);
    }

    public void Continue()
    {
        SaveManager.Instance.CurrentProgress.Money += _reward;
        SaveManager.Instance.CurrentProgress.AllMoney += _reward;
        SaveManager.Instance.CurrentProgress.Kills += EnemySpawner.AllEnemiesCount;
        SaveManager.Instance.CurrentProgress.Level++;
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);

        for (int i = 0; i < 12; i++)
        {
            SaveManager.Instance.CurrentProgress.Experience[i] += _expGain[i];
        }

        LevelController.Instance.LoadMenu();
    }
}
