using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsoleController : MonoBehaviour
{
    [SerializeField] private MenuController _menuController;
    [SerializeField] private GameObject _console;
    [SerializeField] private TMP_InputField _inputField;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            _console.SetActive(!_console.activeInHierarchy);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_console == null) Debug.Log("null");
            if (!_console.activeInHierarchy) return;
            string code = _inputField.text;
            switch (code)
            {
                case "+progress":
                    MaxProgess();
                    break;
                case "-progress":
                    MinProgess();
                    break;
                case "money":
                    AddMoney();
                    break;
            }
            _console.SetActive(false);
        }
    }

    private void MaxProgess()
    {
        Progress maxProgress = new Progress();
        maxProgress.Money = 0;
        maxProgress.Level = 25;
        maxProgress.CharacterLevel = 0;
        maxProgress.DefaultSpellIndexLeft = 0;
        maxProgress.DefaultSpellIndexRight = 0;
        maxProgress.SelectedMap = 0;
        maxProgress.Maps = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            maxProgress.Maps[i] = true;
        }
        maxProgress.Upgrades = new int[24];
        for (int i = 0; i < 24; i++)
        {
            maxProgress.Upgrades[i] = 10;
        }
        maxProgress.Experience = new float[12];
        for (int i = 0; i < 12; i++)
        {
            maxProgress.Experience[i] = 1;
        }
        maxProgress.Achievements = new bool[27];
        for (int i = 0; i < 27; i++)
        {
            maxProgress.Achievements[i] = true;
        }
        maxProgress.AllMoney = 0;
        maxProgress.Kills = 0;
        maxProgress.SFXVolume = 1;
        maxProgress.MusicVolume = 1;
        maxProgress.MouseSensetivity = 1;
        maxProgress.CurrentStats = new StatsController.Stats();
        maxProgress.NoAds = true;
        maxProgress.CoinPremium = true;
        maxProgress.ExpPremium = true;
        maxProgress.Difficulty = 2;
        maxProgress.Init = true;
        SaveManager.Instance.CurrentProgress = maxProgress;
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void MinProgess()
    {
        SaveManager.Instance.CurrentProgress = new Progress();
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void AddMoney()
    {
        SaveManager.Instance.CurrentProgress.Money += 10000;
        _menuController.UpdateMoneyTexts();
        SaveManager.Instance.SaveData(SaveManager.Instance.CurrentProgress);
    }
}
