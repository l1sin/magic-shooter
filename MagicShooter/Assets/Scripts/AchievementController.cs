using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AchievementController.Achievement;

public class AchievementController : MonoBehaviour
{
    public static AchievementController Instance;
    [SerializeField] private bool[] _achievementStates;
    [SerializeField] Achievement[] _achievements;
    [SerializeField] AchievementIcon[] _icons;

    [Header("Achievement focus")]
    [SerializeField] private GameObject _achivementInfo;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;

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

    public void LoadAchievements()
    {
        _achievementStates = SaveManager.Instance.CurrentProgress.Achievements;
        CheckAll();
    }

    public void CheckType(AchievementType type)
    {
        for (int i = 0; i < _achievementStates.Length; i++)
        {
            if (!_achievementStates[i] && _achievements[i].Type == type) _achievementStates[i] = Check(i);
        }
        SaveManager.Instance.CurrentProgress.Achievements = _achievementStates;
        UpdateIcons();
        MenuController.Instance.UpdateAchievementsBar();
    }

    public void CheckAll()
    {
        for (int i = 0; i < _achievementStates.Length; i++)
        {
            if (!_achievementStates[i]) _achievementStates[i] = Check(i);
        }
        SaveManager.Instance.CurrentProgress.Achievements = _achievementStates;
        UpdateIcons();
        MenuController.Instance.UpdateAchievementsBar();
    }

    public void UpdateIcons()
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].UpdateIcon(_achievementStates[i]);
        }
    }

    public bool Check(int index)
    {
        Achievement a = _achievements[index];
        switch (a.Type)
        {
            case AchievementType.Money:
                if (SaveManager.Instance.CurrentProgress.AllMoney >= a.Goal) return true;
                else return false;

            case AchievementType.Kills:
                if (SaveManager.Instance.CurrentProgress.Kills >= a.Goal) return true;
                else return false;

            case AchievementType.Levels:
                if (SaveManager.Instance.CurrentProgress.Level >= a.Goal) return true;
                else return false;

            case AchievementType.Maps:
                int j = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (SaveManager.Instance.CurrentProgress.Maps[i] == true) j++;
                }
                if (j >= a.Goal) return true;
                else return false;

            case AchievementType.Upgrades:
                int k = 0;
                for (int i = 0; i < SaveManager.Instance.CurrentProgress.Upgrades.Length; i++)
                {
                    k += SaveManager.Instance.CurrentProgress.Upgrades[i];
                }
                if (k >= a.Goal) return true;
                else return false;

            case AchievementType.Spells:
                int l = 0;
                for (int i = 0; i < 12; i++)
                {
                    if (SaveManager.Instance.CurrentProgress.Upgrades[i] > 0) l++;
                }
                if (l >= a.Goal) return true;
                else return false;

            case AchievementType.Archmage:
                for (int i = 0; i < a.SpellsIndices.Length; i++)
                {
                    if (SaveManager.Instance.CurrentProgress.Upgrades[a.SpellsIndices[i]] >= 10) continue;
                    else return false;
                }
                return true;

            case AchievementType.Champion:
                int m = 0;
                foreach (bool s in _achievementStates)
                {
                    if (s) m++;
                }
                if (m >= _achievementStates.Length - 1) return true;
                else return false;

            default:
                return false;
        }
    }

    public void FocusAchivement(int index, Sprite sprite)
    {
        _achivementInfo.SetActive(true);
        _image.sprite = sprite;
        
        if (_achievementStates[index])
        {
            _name.text = _achievements[index].Name;
            _description.text = _achievements[index].Description;
        }
        else
        {
            _name.text = "???";
            _description.text = _achievements[index].Hint;
        } 
    }

    [Serializable]
    public class Achievement
    {
        public AchievementType Type;
        public int Goal;
        public int[] SpellsIndices;
        public string Name;
        public string Description;
        public string Hint;

        public enum AchievementType
        {
            Money,
            Kills,
            Levels,
            Maps,
            Upgrades,
            Spells,
            Archmage,
            Champion
        }
    }
}