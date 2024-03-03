using Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    [SerializeField] private UIContorller _ui;
    public bool GameEnd = false;

    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _winScreen;

    [SerializeField] private GameObject _musicPlayer;

    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _winSound;

    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [SerializeField] private SpellButtonGame[] _spellButtons;

    [SerializeField] private SliderController[] _sliders;

    [SerializeField] public float[] ExperienceOnLevel;
    [SerializeField] public int CoinsCollected;
    [SerializeField] private TextMeshProUGUI _coinsCollectedText;

    public void Start()
    {
        Instance = this;
        UnlockSpells();
        LoadSliders();
    }

    public void UnlockSpells()
    {
        for (int i = 0; i < _spellButtons.Length; i++)
        {
            if (SaveManager.Instance.CurrentProgress.Upgrades[i] > 0)
            {
                _spellButtons[i].Unlock();
            }
        }
    }

    public void CollectCoin(int value)
    {
        CoinsCollected += value;
        _coinsCollectedText.text = $"{CoinsCollected}";
    }

    public void Lose()
    {
        GameEnd = true;
        SoundManager.Instance.PlaySound(_deathSound, audioMixerGroup);
        Destroy(_musicPlayer);
        _ui.ShowDeathScreen();
    }

    public void Win()
    {
        GameEnd = true;
        SoundManager.Instance.PlaySound(_winSound, audioMixerGroup);
        Destroy(_musicPlayer);
        _ui.ShowWinScreen();
    } 

    public void LoadMenu()
    {
        PauseManager.SetPause(false);
        SceneManager.LoadScene(SaveManager.Instance.MainMenuSceneIndex);
    }

    public void LoadThisLevel()
    {
        PauseManager.SetPause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadSliders()
    {
        foreach (SliderController s in _sliders)
        {
            s.FindFPSCamera();
            s.LoadSlider();
        }
    }
}
