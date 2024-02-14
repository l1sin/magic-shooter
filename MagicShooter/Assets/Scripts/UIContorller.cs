using UnityEngine;

public class UIContorller : MonoBehaviour
{
    [SerializeField] private GameObject _spellMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private float _slowTimeScale = 0.2f;

    public static bool SpellMenu;
    public static bool PauseMenu;

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

    public void ShowPauseMenu()
    {
        HideSpellMenu();
        PauseMenu = true;
        _pauseMenu.SetActive(true);
        CursorHelper.ShowCursor();
        CharacterInput.AllInputAllowed = false;
    }

    public void HidePauseMenu()
    {
        PauseMenu = false;
        _pauseMenu.SetActive(false);
        CursorHelper.LockAndHideCursor();
        CharacterInput.AllInputAllowed = true;
    }

    public void ShowSpellMenu()
    {
        SpellMenu = true;
        _spellMenu.SetActive(true);
        Time.timeScale = _slowTimeScale;
        CursorHelper.ShowCursor();
        CharacterInput.MouseInputAllowed = false;
    }

    public void HideSpellMenu()
    {
        SpellMenu = false;
        _spellMenu.SetActive(false);
        Time.timeScale = PauseManager.CurrentTimeScale;
        CursorHelper.LockAndHideCursor();
        CharacterInput.MouseInputAllowed = true;
    }

    private void Update()
    {
        CheckSpellMenu();
    }

    public void CheckSpellMenu()
    {
        if (PauseMenu) return;
        if (CharacterInput.SpellMenu) ShowSpellMenu();
        else HideSpellMenu();
    }
}
