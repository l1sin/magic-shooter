using Sounds;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _winScreen;

    [SerializeField] private GameObject _musicPlayer;

    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _winSound;

    [SerializeField] private AudioMixerGroup audioMixerGroup;

    [SerializeField] private SpellButtonGame[] _spellButtons;

    [SerializeField] private SliderController[] _sliders;

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

    public void ShowDeathScreen()
    {
        PauseManager.SetPause(true);
        CursorHelper.ShowCursor();
        _deathScreen.SetActive(true);
        SoundManager.Instance.PlaySound(_deathSound, audioMixerGroup);
        Destroy(_musicPlayer);
        
    }

    public void ShowWinScreen()
    {
        PauseManager.SetPause(true);
        CursorHelper.ShowCursor();
        _winScreen.SetActive(true);
        SoundManager.Instance.PlaySound(_winSound, audioMixerGroup);
        Destroy(_musicPlayer);
    }

    public void LoadMenu()
    {
        PauseManager.SetPause(false);
        SceneManager.LoadScene(SaveManager.Instance.MainMenuSceneIndex);
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
