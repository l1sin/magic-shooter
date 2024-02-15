using Sounds;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UIContorller : MonoBehaviour
{
    [SerializeField] private GameObject _spellMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private float _slowTimeScale = 0.2f;

    public static bool SpellMenu;
    public static bool PauseMenu;

    
    private void Update()
    {
        CheckSpellMenu();
    }

    public void ShowDeathScreen()
    {
        PauseManager.SetPause(true);
        HideAll();
        CursorHelper.ShowCursor();
        _deathScreen.SetActive(true);
        CharacterInput.AllInputAllowed = false;
    }

    public void ShowWinScreen()
    {
        PauseManager.SetPause(true);
        HideAll();
        CursorHelper.ShowCursor();
        _winScreen.SetActive(true);
        CharacterInput.AllInputAllowed = false;
    }

    public void HideAll()
    {
        _spellMenu.SetActive(false);
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _hud.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        if (LevelController.Instance.GameEnd) return;
        HideSpellMenu();
        _hud.SetActive(false);
        PauseMenu = true;
        _pauseMenu.SetActive(true);
        CursorHelper.ShowCursor();
        CharacterInput.AllInputAllowed = false;
    }

    public void HidePauseMenu()
    {
        if (LevelController.Instance.GameEnd) return;
        _hud.SetActive(true);
        PauseMenu = false;
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        CursorHelper.LockAndHideCursor();
        CharacterInput.AllInputAllowed = true;
    }

    public void ShowSpellMenu()
    {
        if (LevelController.Instance.GameEnd) return;
        SpellMenu = true;
        _hud.SetActive(false);
        _spellMenu.SetActive(true);
        Time.timeScale = _slowTimeScale;
        CursorHelper.ShowCursor();
        CharacterInput.MouseInputAllowed = false;
    }

    public void HideSpellMenu()
    {
        if (LevelController.Instance.GameEnd) return;
        SpellMenu = false;
        _hud.SetActive(true);
        _spellMenu.SetActive(false);
        Time.timeScale = PauseManager.CurrentTimeScale;
        CursorHelper.LockAndHideCursor();
        CharacterInput.MouseInputAllowed = true;
    }

    public void CheckSpellMenu()
    {
        if (PauseMenu) return;
        if (CharacterInput.SpellMenu) ShowSpellMenu();
        else HideSpellMenu();
    }

    public void Unpause()
    {
        PauseManager.SetPause(false);
    }

    private void OnEnable()
    {
        PauseManager.PauseOn += ShowPauseMenu;
        PauseManager.PauseOff += HidePauseMenu;
    }

    private void OnDisable()
    {
        PauseManager.PauseOn -= ShowPauseMenu;
        PauseManager.PauseOff -= HidePauseMenu;
    }
}
